using Api.Devion.Models;

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

    public float GetArticlePriceCebeo(string articleReference)
    {
        Item item = SearchForArticleWithReference(articleReference) ?? throw new Exception($"Cebeo has no article with reference = {articleReference}");

        string netPrice = item.UnitPrice?.NetPrice ?? throw new Exception($"Cebeo article with reference = {articleReference} has no netto price");

        return float.Parse(netPrice);
    }
}