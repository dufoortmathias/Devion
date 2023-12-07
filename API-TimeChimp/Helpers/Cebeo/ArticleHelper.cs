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

    public CebeoItem? SearchForArticleWithReference(string articleReference)
    {
        string[] articleReferenceParts = articleReference.Split(' ');
        string reference = articleReferenceParts[0];
        string? reelCode = null;
        string? reelLength = null;
        if (articleReferenceParts.Length > 1)
        {
            reelCode = articleReferenceParts[1];
            reelLength = articleReferenceParts[2];
        }

        CebeoXML cebeoXML = CebeoXML.CreateArticleSearchRequest(reference, config);

        string requestXML = cebeoXML.GetXML();

        var responseXML = webClient.PostAsync("http://b2b.cebeo.be/webservices/xml", requestXML);

        XmlSerializer serializer = new(typeof(CebeoXML));
        using (StringReader reader = new(responseXML))
        {
            CebeoXML result = (CebeoXML)(serializer.Deserialize(reader) ?? throw new Exception($"Request to cebeo failed with xml: \n{requestXML}"));

            return result.Response?.Article?.List?.Item?.Find(x =>
                x.Material != null &&
                x.Material.Reference != null && x.Material.Reference.Equals(reference) &&
                (reelCode == null || (x.Material.ReelCode != null && x.Material.ReelCode.Equals(reelCode))) &&
                (reelLength == null || (x.Material.ReelLength != null && x.Material.ReelLength.Equals(reelLength))));
        }
    }

    public float GetArticlePriceCebeo(string articleReference)
    {
        CebeoItem item = SearchForArticleWithReference(articleReference) ?? throw new Exception($"Cebeo has no article with reference = {articleReference}");

        string netPrice = item.UnitPrice?.NetPrice ?? throw new Exception($"Cebeo article with reference = {articleReference} has no netto price");

        return float.Parse(netPrice);
    }
}