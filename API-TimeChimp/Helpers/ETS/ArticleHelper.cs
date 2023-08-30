namespace Api.Devion.Helpers.ETS;

public class ETSArticleHelper : ETSHelper
{
    WebClient webClient;
    ConfigurationManager config;

    public ETSArticleHelper(FirebirdClientETS clientETS, ConfigurationManager config) : base(clientETS)
    {
        webClient = new();
        this.config = config;
    }

    public ArticleETS GetArticle(string articleNumber)
    {
        string query = "SELECT * FROM CSARTPX WHERE ART_NR = @number";
        Dictionary<string, object> parameters = new()
        {
            {"@number", articleNumber},
        };

        string json = ETSClient.selectQuery(query, parameters) ?? throw new Exception("Error getting artikelnumbers from ETS with query: " + query);

        ArticleETS article = JsonTool.ConvertTo<List<ArticleETS>>(json).First() ?? throw new Exception($"ETS han no article with number = {articleNumber}");

        return article;
    }

    public List<string> GetAriclesCebeo()
    {
        string query = "SELECT CSARTPX.* FROM CSARTPX LEFT JOIN LVPX ON LVPX.LV_COD = CSARTPX.ART_LEV1 WHERE LOWER(LVPX.LV_NAM) LIKE '%cebeo%'";

        string json = ETSClient.selectQuery(query) ?? throw new Exception("Error getting artikelnumbers from ETS with query: " + query);

        List<string?> articles = JsonTool.ConvertTo<List<ArticleETS>>(json).Select(a => a.ART_NR).Distinct().ToList();
        articles.RemoveAll(string.IsNullOrEmpty);

        return articles;
    }

    public string? GetArticleNumberCebeo(string articleReference)
    {
        CebeoXML cebeoXML = CebeoXML.CreateArticleSearchRequest(articleReference, config);

        string requestXML = cebeoXML.GetXML();

        var responseXML = webClient.PostAsync("http://b2b.cebeo.be/webservices/xml", requestXML);

        XmlSerializer serializer = new(typeof(CebeoXML));
        using (StringReader reader = new(responseXML))
        {
            CebeoXML response = (CebeoXML)serializer.Deserialize(reader) ?? throw new Exception($"Request to cebeo failed with xml: \n{requestXML}");

            string? articleNumber = response.Response.Article?.List?.Item?.Find(x => x.Material?.Reference != null && x.Material.Reference.Equals(articleReference))?.Material?.SupplierItemID;

            return articleNumber;
        }
    }

    public float? GetArticlePriceCebeo(string articleNumber)
    {
        CebeoXML cebeoXML = CebeoXML.CreateArticleRequest(articleNumber, config);

        string requestXML = cebeoXML.GetXML();

        var responseXML = webClient.PostAsync("http://b2b.cebeo.be/webservices/xml", requestXML);

        XmlSerializer serializer = new(typeof(CebeoXML));
        using (StringReader reader = new(responseXML))
        {
            CebeoXML response = (CebeoXML)serializer.Deserialize(reader) ?? throw new Exception($"Request to cebeo failed with xml: \n{requestXML}");

            string? netPrice = response.Response.Article.List.Item.FirstOrDefault()?.UnitPrice?.NetPrice;

            return netPrice != null ? float.Parse(netPrice) : null;
        }
    }

    public ArticleETS UpdateArticlePriceETS(string articleNumber, float newPrice, float maxPriceDiff)
    {
        ArticleETS article = GetArticle(articleNumber);
        float price = article.ART_AANKP ?? throw new Exception($"Article with number = {articleNumber}, has no old price assigned");

        if (price - price * maxPriceDiff < newPrice || newPrice < price + price * maxPriceDiff)
        {
            string query = $"EXECUTE PROCEDURE UPDATE_ARTIKEL_PRIJS @number, @price";
            Dictionary<string, object> parameters = new()
            {
                {"@number", articleNumber},
                {"@price", newPrice }
            };

            ETSClient.ExecuteQuery(query, parameters);

            article.ART_AANKP = newPrice;
        }

        return article;
    }
}