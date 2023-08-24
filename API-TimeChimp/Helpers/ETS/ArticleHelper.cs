namespace Api.Devion.Helpers.ETS;

public class ETSArticleHelper : ETSHelper
{
    public ETSArticleHelper(FirebirdClientETS client) : base(client)
    {
    }

    public List<string> GetAriclesSupplier(string supplierId)
    {
        string query = "SELECT DISTINCT ART_NR from csartpx where art_lev1 = @supplier";
        Dictionary<string, object> parameters = new()
        {
            {"@supplier", supplierId},
        };

        string json = ETSClient.selectQuery(query, parameters) ?? throw new Exception("Error getting artikelnumbers from ETS with query: " + query);

        List<string> artikels = JsonTool.ConvertTo<List<Dictionary<string, string>>>(json).Select(d => d["ART_NR"]).ToList();

        return artikels;
    }

    public float? GetArticlePriceCebeo(string articleNumber, ConfigurationManager config)
    {
        CebeoXML cebeoXML = CebeoXML.CreateArticleRequest(articleNumber, config);

        string requestXML = cebeoXML.GetXML(); 

        WebClient client = new();
        var responseXML = client.PostAsync("http://b2b.cebeo.be/webservices/xml", requestXML);

        XmlSerializer serializer = new XmlSerializer(typeof(CebeoXML));
        using (StringReader reader = new StringReader(responseXML))
        {
            CebeoXML response = (CebeoXML) serializer.Deserialize(reader);

            string netPrice = response.Response.Article.List.Item.FirstOrDefault()?.UnitPrice?.NetPrice;

            return netPrice != null ? float.Parse(netPrice) : null;
        }
    }
}