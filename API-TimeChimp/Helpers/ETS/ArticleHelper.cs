namespace Api.Devion.Helpers.ETS;

public class ETSArticleHelper : ETSHelper
{
    WebClient webClient;
    ConfigurationManager config;

    public ETSArticleHelper(FirebirdClientETS clientETS, WebClient client, ConfigurationManager config) : base(clientETS)
    {
        webClient = client;
        this.config = config;
    }

    public ArticleETS GetArticle(string articleReference)
    {
        string query = "SELECT * FROM CSARTPX WHERE ART_LEVREF = @reference";
        Dictionary<string, object> parameters = new()
        {
            {"@reference", articleReference},
        };

        string json = ETSClient.selectQuery(query, parameters) ?? throw new Exception("Error getting artikelnumbers from ETS with query: " + query);

        ArticleETS article = JsonTool.ConvertTo<List<ArticleETS>>(json).First() ?? throw new Exception($"ETS han no article with reference = {articleReference}");

        return article;
    }

    public List<string> GetAriclesSupplier(string supplierId)
    {
        string query = "SELECT * FROM CSARTPX WHERE ART_LEV1 = @supplier";
        Dictionary<string, object> parameters = new()
        {
            {"@supplier", supplierId},
        };

        string json = ETSClient.selectQuery(query, parameters) ?? throw new Exception("Error getting artikelnumbers from ETS with query: " + query);

        List<string?> articles = JsonTool.ConvertTo<List<ArticleETS>>(json).Select(a => a.ART_LEVREF).Distinct().ToList();
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

    public ArticleETS UpdateArticlePriceETS(string articleReference, float newPrice, float maxPriceDiff)
    {
        ArticleETS article = GetArticle(articleReference);
        float price = article.ART_AANKP ?? throw new Exception($"Article with reference = {articleReference}, has no old price assigned");

        if (price - price * maxPriceDiff < newPrice || newPrice < price + price * maxPriceDiff)
        {
            string query = $"UPDATE CSARTPX SET PLA_ART_AANKP = @price WHERE ART_LEVREF = @reference";
            Dictionary<string, object> parameters = new()
            {
                {"@reference", articleReference},
                {"@price", newPrice }
            };

            // ETSClient.updateQuery(query, parameters);

            article.ART_AANKP = newPrice;
        }

        return article;
    }
}