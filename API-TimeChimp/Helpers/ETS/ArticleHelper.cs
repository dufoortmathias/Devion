namespace Api.Devion.Helpers.ETS;

public class ETSArticleHelper : ETSHelper
{

    public ETSArticleHelper(FirebirdClientETS clientETS) : base(clientETS)
    {
    }

    public ArticleETS? GetArticle(string articleNumber)
    {
        string query = "SELECT * FROM CSARTPX WHERE ART_NR = @number";
        Dictionary<string, object> parameters = new()
        {
            {"@number", articleNumber},
        };

        string json = ETSClient.selectQuery(query, parameters) ?? throw new Exception("Error getting artikelnumbers from ETS with query: " + query);

        ArticleETS? article = JsonTool.ConvertTo<List<ArticleETS>>(json).FirstOrDefault();

        return article;
    }

    public string? GetArticleReference(string articleNumber, string supplierId)
    {
        string query = "SELECT CO_REFLEV FROM CONTACT3 WHERE CO_ARTNR = @number AND CO_LEVNR = @supplier";
        Dictionary<string, object> parameters = new()
            {
                {"@number", articleNumber},
                {"@supplier", supplierId}
            };

        string json = ETSClient.selectQuery(query, parameters) ?? throw new Exception("Error getting article reference from ETS with query: " + query);
        string? articleReference = JsonTool.ConvertTo<List<Dictionary<string, string>>>(json).FirstOrDefault()?["CO_REFLEV"];
        articleReference = string.IsNullOrEmpty(articleReference) ? null : articleReference;

        if (articleReference == null)
        {
            ArticleETS? article = GetArticle(articleNumber);
            if (article != null && article.ART_LEV1 != null && article.ART_LEV1.Equals(supplierId) && article.ART_LEVREF != null)
            {
                return article.ART_LEVREF;
            }
        }

        return articleReference;
    }

    public bool ArticleWithReferenceExists(string articleReference, string supplierId)
    {
        // Search for article with reference in table CSARTPX
        string query = "SELECT * FROM CSARTPX WHERE ART_LEVREF = @number AND ART_LEV1 = @supplier";
        Dictionary<string, object> parameters = new()
        {
            {"@number", articleReference},
            {"@supplier", supplierId}
        };

        string json = ETSClient.selectQuery(query, parameters) ?? throw new Exception("Error getting article from ETS with query: " + query);

        if (JsonTool.ConvertTo<List<ArticleETS>>(json).Count > 0)
        {
            return true;
        }

        // Search for article with reference in table CONTACT3
        query = "SELECT * FROM CONTACT3 WHERE CO_REFLEV = @number AND CO_LEVNR = @supplier";
        parameters = new()
        {
            {"@number", articleReference},
            {"@supplier", supplierId}
        };

        json = ETSClient.selectQuery(query, parameters) ?? throw new Exception("Error getting article from ETS with query: " + query);

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

    public List<string> GetAricles(string supplierId)
    {
        string query = "SELECT CSARTPX.* FROM CSARTPX LEFT JOIN LVPX ON LVPX.LV_COD = CSARTPX.ART_LEV1 WHERE ART_LEV1 = @supplierId";

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
        ArticleETS article = GetArticle(articleNumber) ?? throw new Exception($"ETS han no article with number = {articleNumber}");
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

    public void LinkArticle(string articleNumberMain, string articleNumberToLink)
    {
        Console.WriteLine($"{articleNumberToLink} => {articleNumberMain}");
    }
}