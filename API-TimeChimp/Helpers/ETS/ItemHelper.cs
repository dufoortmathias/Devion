namespace Api.Devion.Helpers.ETS;

public class ETSItemHelper : ETSHelper
{

    public ETSItemHelper(FirebirdClientETS clientETS) : base(clientETS)
    {
    }

    public Dictionary<string, string> UpdateItem(ItemChange item)
    {
        Console.WriteLine($"Updating item: {item.ArticleNumber}");
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
        Console.WriteLine($"Creating new item: {item.ArtikelNr}");
        var query = "execute procedure INSERT_ARTIKEL_WS(@ARTIKELNR, @IN_OMSCHRIJVING, @IN_OMSCHRIJVING2, @IN_REFERENTIE, @IN_FAMILIE, @IN_SUBFAMILIE, @IN_SUBSUBFAMILIE, @IN_BINNENKLEUR, @IN_BUITENKLEUR, @IN_BEREKENINGSWIJZE, @IN_LENGTE, @IN_BREEDTE, @IN_HOOGTE, @IN_CONVERSIEFACTOR, @IN_CONVERSIEFACTOR_TYPE, @IN_EENHEID_AANKOOP, @IN_EENHEID_VERKOOP, @IN_AKP, @IN_VERKP, @IN_AKP_VERBR_EENHEID, @IN_VKP_VERBR_EENH, @IN_ACTIEF, @IN_STOCK_ACTIEF, @IN_HOOFDLEVERANCIER, @IN_SAPA_ARTIKELNR, @IN_IS_SAPA_INSERT)";

        Dictionary<string, object?> parameters = new()
        {
            {"@ARTIKELNR", item.ArtikelNr },
            {"@IN_OMSCHRIJVING", item.Omschrijving },
            {"@IN_OMSCHRIJVING2", "" },
            {"@IN_REFERENTIE", item.Reflev },
            {"@IN_FAMILIE", item.Familie },
            {"@IN_SUBFAMILIE", item.Subfamilie },
            {"@IN_SUBSUBFAMILIE", null },
            {"@IN_BINNENKLEUR", null },
            {"@IN_BUITENKLEUR", null },
            {"@IN_BEREKENINGSWIJZE", null },
            {"@IN_LENGTE", float.Parse(item.Lengte) },
            {"@IN_BREEDTE", float.Parse(item.Breedte) },
            {"@IN_HOOGTE", float.Parse(item.Hoogte) },
            {"@IN_CONVERSIEFACTOR", float.Parse(item.Omrekfac) },
            {"@IN_CONVERSIEFACTOR_TYPE", item.Typfac },
            {"@IN_EENHEID_AANKOOP", item.Aaneh },
            {"@IN_EENHEID_VERKOOP", item.Vereh },
            {"@IN_AKP", float.Parse(item.Aankoop) },
            {"@IN_VERKP", float.Parse(item.Verkoop) },
            {"@IN_AKP_VERBR_EENHEID", null },
            {"@IN_VKP_VERBR_EENH", null },
            {"@IN_ACTIEF", 1 },
            {"@IN_STOCK_ACTIEF", 1 },
            {"@IN_HOOFDLEVERANCIER",  "ANCON"},
            {"@IN_SAPA_ARTIKELNR",  null },
            {"@IN_IS_SAPA_INSERT", null },
        };

        try
        {
            ETSClient.ExecuteQuery(query, parameters);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        Dictionary<string, string> logDict = new()
            {
                {"artikelNumber", item.ArtikelNr },
                { "action", "create" }
            };
        return logDict;
    }
}