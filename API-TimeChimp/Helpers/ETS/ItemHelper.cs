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
}