namespace Api.Devion.Helpers.ETS;

public class CebeoArticleHelper
{
    private WebClient webClient;
    private ConfigurationManager config;

    public CebeoArticleHelper(ConfigurationManager config)
    {
        this.config = config;
        webClient = new();
    }

    public Item? SearchForArticleWithReference(string articleReference)
    {
        CebeoXML cebeoXML = CebeoXML.CreateArticleSearchRequest(articleReference, config);

        string requestXML = cebeoXML.GetXML();

        var responseXML = webClient.PostAsync("http://b2b.cebeo.be/webservices/xml", requestXML);

        XmlSerializer serializer = new(typeof(CebeoXML));
        using (StringReader reader = new(responseXML))
        {
            CebeoXML result = (CebeoXML) (serializer.Deserialize(reader) ?? throw new Exception($"Request to cebeo failed with xml: \n{requestXML}"));

            return result.Response?.Article?.List?.Item?.Find(x => x.Material?.Reference != null && x.Material.Reference.Equals(articleReference));
        }
    }

    public string? GetArticleNumberCebeo(string articleReference)
    {
        Item article = SearchForArticleWithReference(articleReference) ?? throw new Exception($"Cebeo has no article with reference = {articleReference}");

        string? articleNumber = article?.Material?.SupplierItemID;

        return articleNumber;
    }

    public float? GetArticlePriceCebeo(string articleNumber)
    {
        CebeoXML cebeoXML = CebeoXML.CreateArticleRequest(articleNumber, config);

        string requestXML = cebeoXML.GetXML();

        var responseXML = webClient.PostAsync("http://b2b.cebeo.be/webservices/xml", requestXML);

        XmlSerializer serializer = new(typeof(CebeoXML));
        using (StringReader reader = new(responseXML))
        {
            CebeoXML response = (CebeoXML) (serializer.Deserialize(reader) ?? throw new Exception($"Request to cebeo failed with xml: \n{requestXML}"));

            string? netPrice = response.Response?.Article?.List?.Item?.FirstOrDefault()?.UnitPrice?.NetPrice;

            return netPrice != null ? float.Parse(netPrice) : null;
        }
    }
}