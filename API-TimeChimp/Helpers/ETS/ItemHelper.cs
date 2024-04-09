namespace Api.Devion.Helpers.ETS;

public class ETSItemHelper : ETSHelper
{

    public ETSItemHelper(FirebirdClientETS clientETS) : base(clientETS)
    {
    }

    public Dictionary<string, string> UpdateItem(ItemChange item)
    {
        var query = $"update CSARTPX set {item.Key} = @value where ART_NR = @articleNumber";

        Dictionary<string, object?> parameters = new()
        {
            { "@articleNumber", item.ArticleNumber },
            { "@key", item.Key },
            { "@value", item.Change?.NewWaarde }
        };

        ETSClient.ExecuteQuery(query, parameters);
        Dictionary<string, string> logDict = new()
            {
                {"artikelNumber", item.ArticleNumber },
                {"action", "update" },
                {"extra", "changed " + item.Key }
            };
        return logDict;
    }

    public Dictionary<string, string> CreateItem(NewItem item)
    {
        var query = "EXECUTE PROCEDURE INSERT_ARTIKEL_WS @IN_ARTIKELNR, @IN_OMSCHRIJVING, @IN_OMSCHRIJVING2, @IN_REFERENTIE, @IN_FAMILIE, @IN_SUBFAMILIE, @IN_SUBSUBFAMILIE, @IN_BINNENKLEUR, @IN_BUITENKLEUR, @IN_BEREKENINGSWIJZE, @IN_LENGTE, @IN_BREEDTE, @IN_HOOGTE, @IN_CONVERSIEFACTOR, @IN_CONVERSIEFACTOR_TYPE, @IN_EENHEID_AANKOOP, @IN_EENHEID_VERKOOP, @IN_AKP, @IN_VERKP, @IN_AKP_VERBR_EENHEID, @IN_VKP_VERBR_EENH, @IN_ACTIEF, @IN_STOCK_ACTIEF, @IN_HOOFDLEVERANCIER, @IN_SAPA_ARTIKELNR, @IN_IS_SAPA_INSERT";

        var akp_verbr_eenheid = 0.0f;
        var vkp_verbr_eenh = 0.0f;
        if (item.Typfac == "0")
        {
            akp_verbr_eenheid = float.Parse(item.Aankoop.Replace('.', ',')) / float.Parse(item.Omrekfac.Replace('.', ','));
            vkp_verbr_eenh = float.Parse(item.Verkoop.Replace('.', ',')) / float.Parse(item.Omrekfac.Replace('.', ','));
        }
        else if (item.Typfac == "1")
        {
            akp_verbr_eenheid = float.Parse(item.Aankoop.Replace('.', ',')) * float.Parse(item.Omrekfac.Replace('.', ','));
            vkp_verbr_eenh = float.Parse(item.Verkoop.Replace('.', ',')) * float.Parse(item.Omrekfac.Replace('.', ','));
        }

        if (item.Hoofdleverancier != "")
        {
            var query2 = $"select lv_cod from lvpx where lv_nam = '{item.Hoofdleverancier}'";
            var parameters2 = new Dictionary<string, object?>()
            {
                {"@hoofdleverancier", item.Hoofdleverancier }
            };
            var result = ETSClient.selectQuery(query2, parameters2);

            var json = JsonTool.ConvertTo<List<Dictionary<string, string>>>(result);
            item.Hoofdleverancier = json[0]["LV_COD"];
        }

        //check if exists
        var queryCheck = "select ar_cod from artfam";
        var family = ETSClient.selectQuery(queryCheck);
        var jsonFamily = JsonTool.ConvertTo<List<Dictionary<string, string>>>(family);
        var exists = jsonFamily.Any(x => x["AR_COD"] == item.Familie);
        if (!exists)
        {
            item.Familie = null;
            item.Subfamilie = null;
        }

        var queryCheck2 = "select asf_cod from artsubfam";
        var subfamily = ETSClient.selectQuery(queryCheck2);
        var jsonSubFamily = JsonTool.ConvertTo<List<Dictionary<string, string>>>(subfamily);
        var exists2 = jsonSubFamily.Any(x => x["ASF_COD"] == item.Subfamilie);
        if (!exists2)
        {
            item.Subfamilie = null;
        }

        Dictionary<string, object?> parameters = new()
        {
            {"@IN_ARTIKELNR", item.ArtikelNr },
            {"@IN_OMSCHRIJVING", item.Omschrijving },
            {"@IN_OMSCHRIJVING2", "" },
            {"@IN_REFERENTIE", item.Reflev },
            {"@IN_FAMILIE", item.Familie },
            {"@IN_SUBFAMILIE", item.Subfamilie },
            {"@IN_SUBSUBFAMILIE", 0 },
            {"@IN_BINNENKLEUR", "" },
            {"@IN_BUITENKLEUR", "" },
            {"@IN_BEREKENINGSWIJZE", 0 },
            {"@IN_LENGTE", float.Parse(item.Lengte.Replace('.',',')) },
            {"@IN_BREEDTE", float.Parse(item.Breedte.Replace('.',',')) },
            {"@IN_HOOGTE", float.Parse(item.Hoogte.Replace('.',',')) },
            {"@IN_CONVERSIEFACTOR", float.Parse(item.Omrekfac) },
            {"@IN_CONVERSIEFACTOR_TYPE", item.Typfac },
            {"@IN_EENHEID_AANKOOP", item.Aaneh },
            {"@IN_EENHEID_VERKOOP", item.Vereh },
            {"@IN_AKP", float.Parse(item.Aankoop.Replace('.',',')) },
            {"@IN_VERKP", float.Parse(item.Verkoop.Replace('.',',')) },
            {"@IN_AKP_VERBR_EENHEID", akp_verbr_eenheid },
            {"@IN_VKP_VERBR_EENH", vkp_verbr_eenh },
            {"@IN_ACTIEF", 1 },
            {"@IN_STOCK_ACTIEF", 1 },
            {"@IN_HOOFDLEVERANCIER",  item.Hoofdleverancier},
            {"@IN_SAPA_ARTIKELNR",  null },
            {"@IN_IS_SAPA_INSERT", 0 },
        };

        var query3 = "update CSARTPX set ART_GEWICHT = @mass, ART_AANKOOP_PER = @minaan, ART_MERK = @merk, ART_WPROC = @winstper where ART_NR = @articleNumber";
        Dictionary<string, object?> parameters3 = new()
        {
            {"@mass", float.Parse(item.Mass.Replace('.', ',')) },
            {"@minaan", int.Parse(item.Minaan) },
            {"@merk", item.Merk },
            {"@winstper", float.Parse(item.Winstpercentage.Replace('.', ',')) },
            {"@articleNumber", item.ArtikelNr }
        };

        var query4 = "execute procedure UPDATE_ARTIKEL_PRIJS @IN_ARTNR, @IN_AANKP";
        Dictionary<string, object?> parameters4 = new()
        {
            {"@IN_ARTNR", item.ArtikelNr },
            {"@IN_AANKP", float.Parse(item.Aankoop.Replace('.', ',')) }
        };

        try
        {
            ETSClient.ExecuteQuery(query, parameters);
            ETSClient.ExecuteQuery(query3, parameters3);
            ETSClient.ExecuteQuery(query4, parameters4);
            Dictionary<string, string> logDict = new()
            {
                {"artikelNumber", item.ArtikelNr },
                { "action", "create" }
            };
            return logDict;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            Dictionary<string, string> logDict = new()
            {
                {"artikelNumber", item.ArtikelNr },
                { "action", "create" },
                { "error", e.Message }
            };
            return logDict;
        }
    }
}