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
        Dictionary<string, object> parameters = new()
        {
            {"@supplierId", supplierId},
        };

        string json = ETSClient.selectQuery(query, parameters) ?? throw new Exception("Error getting artikelnumbers from ETS with query: " + query);

        List<string> articles = JsonTool.ConvertTo<List<ArticleETS>>(json)
            .Select(a => a.ART_NR).Distinct()
            .Where(x => x != null)
            .Cast<string>()
            .ToList();

        return articles;
    }

    public Dictionary<string, List<string>> ValidateArticle(ArticleWeb article)
    {
        Dictionary<string, List<string>> problems = new()
            {
                {"number", new() },
                {"reference", new() },
                {"description", new() },
                {"brand", new() },
                {"unitOfMeasure", new() },
                {"salesPackQuantity", new() },
                {"nettoPrice", new() },
                {"tarifPrice", new() },
                {"url", new() }
        };

        if (string.IsNullOrEmpty(article.Number))
        {
            problems["number"].Add("Can't be empty");
        }
        else if (!article.Number.Equals(article.Number.ToUpper()))
        {
            problems["number"].Add("Articlenumber contains lower case characters");
        }
        else if (ArticleWithNumberExists(article.Number))
        {
            problems["number"].Add("Articlenumber already exists in ETS");
        }

        if (string.IsNullOrEmpty(article.Reference))
        {
            problems["reference"].Add("Can't be empty");
        }

        if (string.IsNullOrEmpty(article.Description))
        {
            problems["description"].Add("Can't be empty");
        }

        if (string.IsNullOrEmpty(article.Brand))
        {
            problems["brand"].Add("Can't be empty");
        }

        string query = "SELECT EH_COD AS CODE, EH_OMS1 AS DESCRIPTION, EH_OMS2 AS SHORT_DESCRIPTION FROM EENHEID";
        string[] MeasureTypes = JsonTool.ConvertTo<List<Dictionary<string, string>>>(ETSClient.selectQuery(query)).Select(x => x["CODE"]).ToArray();
        if (string.IsNullOrEmpty(article.UnitOfMeasure))
        {
            problems["unitOfMeasure"].Add("Can't be empty");
        }
        else if (!MeasureTypes.Contains(article.UnitOfMeasure))
        {
            problems["unitOfMeasure"].Add("Chosen value not valid");
        }

        if (article.SalesPackQuantity < 0)
        {
            problems["salesPackQuantity"].Add("Can't be below zero");
        }

        if (article.NettoPrice < 0)
        {
            problems["nettoPrice"].Add("Price can't be below zero");
        }
        else if (article.TarifPrice < 0)
        {
            problems["tarifPrice"].Add("Price can't be below zero");
        }
        else if (article.TarifPrice < article.NettoPrice)
        {
            problems["nettoPrice"].Add("Price can't be higher then tarif price");
            problems["tarifPrice"].Add("Price can't be lower then netto price");
        }

        if (string.IsNullOrEmpty(article.URL))
        {
            problems["url"].Add("Can't be empty");
        }

        return problems;
    }

    public ArticleWeb CreateArticle(ArticleWeb article)
    {
        if (ValidateArticle(article).Any(x => x.Value.Count > 0))
        {
            throw new Exception("Some fields from article given are not valid");
        }

        string createQuery = "EXECUTE PROCEDURE INSERT_ARTIKEL_WS @number, @description, null, @reference, @familie, @subfamilie, null, null, null, null, @lengte, @breedte, @hoogte, null, null, null, null, @aankoop, @verkoop, null, null, 1, 1, @supplier, null, null";
        Dictionary<string, object> createParameters = new()
        {
            { "@number", article.Number },
            { "@description", article.Description },
            { "@reference", article.Reference },
            { "@familie", null },
            { "@subfamilie", null },
            { "@lengte", 0 },
            { "@breedte", 0 },
            { "@hoogte", 0 },
            { "@aankoop", article.NettoPrice },
            { "@verkoop", article.NettoPrice },
            { "@supplier", ""}
        };

        ETSClient.ExecuteQuery(createQuery, createParameters);

        //update query
        string updateQuery = $"UPDATE CSARTPX SET ART_HYPERLINK = @url WHERE ART_NR = @id;";
        Dictionary<string, object> updateParameters = new()
        {
            { "@id", article.Number },
            { "@url", article.URL }
        };

        ETSClient.ExecuteQuery(updateQuery, updateParameters);

        return article;
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

    public void LinkArticle(string articleNumberMaster, string articleNumberToLink)
    {
        string query = $"SELECT COUNT(*) FROM TBL_ARTIKEL_GEKOPPELD AS L LEFT JOIN CSARTPX AS A1 ON A1.ART_ID = L.ARG_ART_ID LEFT JOIN CSARTPX AS A2 ON A2.ART_ID = L.ARG_MASTER_ID WHERE A1.ART_NR = @artikel AND A2.ART_NR = @master";
        Dictionary<string, object> parameters = new()
        {
            {"@artikel", articleNumberToLink},
            {"@master", articleNumberMaster }
        };

        string response = ETSClient.selectQuery(query, parameters);
        int count = JsonTool.ConvertTo<List<Dictionary<string, int>>>(response).First()["COUNT"];

        if (count == 0)
        {
            // TODO create link ETS
        }
    }
}