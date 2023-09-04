namespace Api.Devion.Helpers.ETS;

public class ETSArticleHelper : ETSHelper
{

    public ETSArticleHelper(FirebirdClientETS clientETS) : base(clientETS)
    {
    }

    public ArticleETS GetArticle(string articleNumber)
    {
        string query = "SELECT * FROM CSARTPX WHERE ART_NR = @number";
        Dictionary<string, object> parameters = new()
        {
            {"@number", articleNumber},
        };

        string json = ETSClient.selectQuery(query, parameters) ?? throw new Exception("Error getting artikelnumbers from ETS with query: " + query);

        ArticleETS article = JsonTool.ConvertTo<List<ArticleETS>>(json).FirstOrDefault() ?? throw new Exception($"ETS han no article with number = {articleNumber}");

        return article;
    }

    public string GetArticleReference(string articleNumber, string supplierId)
    {
        ArticleETS? article = null;
        try
        {
            GetArticle(articleNumber);
        }
        catch
        {

        }

        string? articleReference;
        if (article != null && article.ART_LEV1 != null && article.ART_LEV1.Equals(supplierId) && article.ART_LEVREF != null)
        {
            return article.ART_LEVREF;
        }
        else
        {
            string query = "SELECT CO_REFLEV FROM CONTACT3 WHERE CO_ARTNR = @number AND CO_LEVNR = @supplier";
            Dictionary<string, object> parameters = new()
            {
                {"@number", articleNumber},
                {"@supplier", supplierId}
            };

            string json = ETSClient.selectQuery(query, parameters) ?? throw new Exception("Error getting article reference from ETS with query: " + query);
            articleReference = JsonTool.ConvertTo<List<Dictionary<string, string>>>(json).FirstOrDefault()?["CO_REFLEV"];
        }

        return articleReference ?? articleNumber;
    }

    public bool ArticleWithReferenceExists(string articleReference)
    {
        string query = "SELECT * FROM CSARTPX WHERE ART_LEVREF = @number";
        Dictionary<string, object> parameters = new()
        {
            {"@number", articleReference},
        };

        string json = ETSClient.selectQuery(query, parameters) ?? throw new Exception("Error getting article from ETS with query: " + query);

        return JsonTool.ConvertTo<List<ArticleETS>>(json).Count > 0;
    }

    public bool ArticleWithNumberExists(string articleNumber)
    {
        string query = "SELECT * FROM CSARTPX WHERE ART_NR = @number";
        Dictionary<string, object> parameters = new()
        {
            {"@number", articleNumber},
        };

        string json = ETSClient.selectQuery(query, parameters) ?? throw new Exception("Error getting article from ETS with query: " + query);

        return JsonTool.ConvertTo<List<ArticleETS>>(json).Count > 0;
    }

    public List<string> GetAriclesCebeo()
    {
        string query = "SELECT CSARTPX.* FROM CSARTPX LEFT JOIN LVPX ON LVPX.LV_COD = CSARTPX.ART_LEV1 WHERE LOWER(LVPX.LV_NAM) LIKE '%cebeo%'";

        string json = ETSClient.selectQuery(query) ?? throw new Exception("Error getting artikelnumbers from ETS with query: " + query);

        List<string> articles = JsonTool.ConvertTo<List<ArticleETS>>(json)
            .Select(a => a.ART_NR).Distinct()
            .Where(x => x != null)
            .Cast<string>()
            .ToList();

        return articles;
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