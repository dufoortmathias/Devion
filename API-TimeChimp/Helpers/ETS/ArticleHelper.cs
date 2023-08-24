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

    public List<string> GetAriclesSupplier(string supplierId)
    {
        string query = "SELECT DISTINCT ART_LEVREF from csartpx where art_lev1 = @supplier";
        Dictionary<string, object> parameters = new()
        {
            {"@supplier", supplierId},
        };

        string json = ETSClient.selectQuery(query, parameters) ?? throw new Exception("Error getting artikelnumbers from ETS with query: " + query);

        List<string> artikels = JsonTool.ConvertTo<List<Dictionary<string, string>>>(json).Select(d => d["ART_LEVREF"]).ToList();

        return artikels;
    }

    public string GetArticleNumberCebeo(string articleReference)
    {
        CebeoXML cebeoXML = CebeoXML.CreateArticleSearchRequest(articleReference, config);

        string requestXML = cebeoXML.GetXML();

        var responseXML = webClient.PostAsync("http://b2b.cebeo.be/webservices/xml", requestXML);

        XmlSerializer serializer = new XmlSerializer(typeof(CebeoXML));
        using (StringReader reader = new StringReader(responseXML))
        {
            CebeoXML response = (CebeoXML)serializer.Deserialize(reader) ?? throw new Exception($"Request to cebeo failed with xml: \n{requestXML}");

            string artikelNumber = response.Response.Article.List.Item.FirstOrDefault()?.Material?.SupplierItemID;

            return artikelNumber;
        }
    }

    public float? GetArticlePriceCebeo(string articleNumber)
    {
        CebeoXML cebeoXML = CebeoXML.CreateArticleRequest(articleNumber, config);

        string requestXML = cebeoXML.GetXML();

        var responseXML = webClient.PostAsync("http://b2b.cebeo.be/webservices/xml", requestXML);

        XmlSerializer serializer = new XmlSerializer(typeof(CebeoXML));
        using (StringReader reader = new StringReader(responseXML))
        {
            CebeoXML response = (CebeoXML)serializer.Deserialize(reader) ?? throw new Exception($"Request to cebeo failed with xml: \n{requestXML}");

            string? netPrice = response.Response.Article.List.Item.FirstOrDefault()?.UnitPrice?.NetPrice;

            return netPrice != null ? float.Parse(netPrice) : null;
        }
    }
}