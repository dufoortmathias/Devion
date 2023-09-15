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
                {"artikelNr", new() },
                {"reflev", new() },
                {"omschrijving", new() },
                {"merk", new() },
                {"aaneh", new() },
                {"minaan", new() },
                {"aankoop", new() },
                {"tarief", new() },
                {"link", new() }
        };

        if (string.IsNullOrEmpty(article.artikelNr))
        {
            problems["artikelNr"].Add("Can't be empty");
        }
        else if (!article.artikelNr.Equals(article.artikelNr.ToUpper()))
        {
            problems["artikelNr"].Add("Articlenumber contains lower case characters");
        }
        else if (ArticleWithNumberExists(article.artikelNr))
        {
            problems["artikelNr"].Add("Articlenumber already exists in ETS");
        }

        if (string.IsNullOrEmpty(article.reflev))
        {
            problems["reflev"].Add("Can't be empty");
        }

        if (string.IsNullOrEmpty(article.omschrijving))
        {
            problems["omschrijving"].Add("Can't be empty");
        }

        if (string.IsNullOrEmpty(article.merk))
        {
            problems["merk"].Add("Can't be empty");
        }

        string query = "SELECT EH_COD AS CODE, EH_OMS1 AS DESCRIPTION, EH_OMS2 AS SHORT_DESCRIPTION FROM EENHEID";
        string[] MeasureTypes = JsonTool.ConvertTo<List<Dictionary<string, string>>>(ETSClient.selectQuery(query)).Select(x => x["CODE"]).ToArray();
        if (string.IsNullOrEmpty(article.aaneh))
        {
            problems["aaneh"].Add("Can't be empty");
        }
        else if (!MeasureTypes.Contains(article.aaneh))
        {
            problems["aaneh"].Add("Chosen value not valid");
        }

        if (article.minaan < 0)
        {
            problems["minaan"].Add("Can't be below zero");
        }

        if (article.aankoop < 0)
        {
            problems["aankoop"].Add("Price can't be below zero");
        }
        else if (article.tarief < 0)
        {
            problems["tarief"].Add("Price can't be below zero");
        }
        else if (article.tarief < article.aankoop)
        {
            problems["aankoop"].Add("Price can't be higher then tarif price");
            problems["tarief"].Add("Price can't be lower then netto price");
        }

        if (string.IsNullOrEmpty(article.link))
        {
            problems["link"].Add("Can't be empty");
        }

        return problems;
    }

    public ArticleWeb CreateArticle(ArticleWeb article)
    {
        if (ValidateArticle(article).Any(x => x.Value.Count > 0))
        {
            throw new Exception("Some fields from article given are not valid");
        }

        string createQuery = "EXECUTE PROCEDURE INSERT_ARTIKEL_WS @number, @description, null, @reference, @familie, @subfamilie, null, null, null, null, @lengte, @breedte, @hoogte, @omrekfac, @typfac, @aaneh, null, @aankoop, @verkoop, @aankoopEenh, @verkoopEenh, 1, 1, @supplier, null, null";
        Dictionary<string, object?> createParameters = new()
        {
            { "@number", article.artikelNr },
            { "@description", article.omschrijving },
            { "@reference", article.reflev },
            { "@familie", article.familie },
            { "@subfamilie", article.subfamilie },
            { "@lengte", article.lengte },
            { "@breedte", article.breedte },
            { "@hoogte", article.hoogte },
            { "@omrekfac", article.omrekfac },
            { "@typfac", article.typfac },
            { "@aaneh", article.aaneh },
            { "@aankoop", article.aankoop },
            { "@verkoop", article.verkoop },
            { "@aankoopEenh", article.aankoop },
            { "@verkoopEenh", article.verkoop },
            { "@supplier", article.hoofdleverancier}
        };

        ETSClient.ExecuteQuery(createQuery, createParameters);

        //update query
        string updateQuery = $"UPDATE CSARTPX SET ART_MERK = @merk, ART_KORT = @korting, ART_HYPERLINK = @url WHERE ART_NR = @id;";
        Dictionary<string, object?> updateParameters = new()
        {
            { "@id", article.artikelNr },
            { "@url", article.link },
            { "@merk", article.merk },
            { "@korting", article.stdKorting }
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
            // TODO  (Create query) to link articles in ETS -> Table: TBL_ARTIKEL_GEKOPPELD
        }
    }
}