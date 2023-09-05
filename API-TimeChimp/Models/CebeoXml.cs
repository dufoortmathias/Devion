using System.Xml.Schema;

namespace Api.Devion.Models;

[XmlRoot(ElementName = "Customer")]
public class Customer
{
    public Customer() {}

    public Customer(ConfigurationManager config)
    {
        CustomerNumber = config["Cebeo:CustomerNumber"];
        UserName = config["Cebeo:UserName"];
        Password = config["Cebeo:Password"];

    }

    [XmlElement(ElementName = "CustomerNumber")]
    public string? CustomerNumber { get; set; }

    [XmlElement(ElementName = "UserName")]
    public string? UserName { get; set; }

    [XmlElement(ElementName = "Password")]
    public string? Password { get; set; }
}

[XmlRoot(ElementName = "UnitPrice")]
public class UnitPrice
{

    [XmlElement(ElementName = "TarifPrice")]
    public string? TarifPrice { get; set; }

    [XmlElement(ElementName = "NetPrice")]
    public string? NetPrice { get; set; }

    [XmlElement(ElementName = "TAV")]
    public string? TAV { get; set; }
}

[XmlRoot(ElementName = "Material")]
public class Material
{
    public Material() { }

    public Material(string artikelNummer)
    {
        Reference = artikelNummer;
    }

    [XmlElement(ElementName = "SupplierItemID")]
    public string? SupplierItemID { get; set; }

    [XmlElement(ElementName = "Reference")]
    public string? Reference { get; set; }

    [XmlElement(ElementName = "Description")]
    public string? Description { get; set; }

    [XmlElement(ElementName = "BrandName")]
    public string? BrandName { get; set; }
}

[XmlRoot(ElementName = "OrderDate")]
public class OrderDate
{

    [XmlElement(ElementName = "Day")]
    public int? Day { get; set; }

    [XmlElement(ElementName = "Month")]
    public int? Month { get; set; }

    [XmlElement(ElementName = "Year")]
    public int? Year { get; set; }
}

[XmlRoot(ElementName = "RequestedDeliveryDate")]
public class RequestedDeliveryDate
{

    [XmlElement(ElementName = "Day")]
    public int? Day { get; set; }

    [XmlElement(ElementName = "Month")]
    public int? Month { get; set; }

    [XmlElement(ElementName = "Year")]
    public int? Year { get; set; }
}

[XmlRoot(ElementName = "BOEstimatedDeliveryDate")]
public class BOEstimatedDeliveryDate
{

    [XmlElement(ElementName = "Day")]
    public int? Day { get; set; }

    [XmlElement(ElementName = "Month")]
    public int? Month { get; set; }

    [XmlElement(ElementName = "Year")]
    public int? Year { get; set; }
}

[XmlRoot(ElementName = "DeliveryDate")]
public class DeliveryDate
{

    [XmlElement(ElementName = "Day")]
    public int? Day { get; set; }

    [XmlElement(ElementName = "Month")]
    public int? Month { get; set; }

    [XmlElement(ElementName = "Year")]
    public int? Year { get; set; }
}

[XmlRoot(ElementName = "DeliveryAddress")]
public class DeliveryAddress
{

    [XmlElement(ElementName = "DeliverTo")]
    public string? DeliverTo { get; set; }

    [XmlElement(ElementName = "Street")]
    public string? Street { get; set; }

    [XmlElement(ElementName = "City")]
    public string? City { get; set; }

    [XmlElement(ElementName = "PostalCode")]
    public int? PostalCode { get; set; }
}

[XmlRoot(ElementName = "DeliveryLocation")]
public class DeliveryLocation
{

    [XmlElement(ElementName = "DeliveryAddress")]
    public DeliveryAddress? DeliveryAddress { get; set; }

    [XmlElement(ElementName = "ContactPerson")]
    public string? ContactPerson { get; set; }

    [XmlElement(ElementName = "ContactTelephone")]
    public string? ContactTelephone { get; set; }
}

[XmlRoot(ElementName = "OrderHeader")]
public class OrderHeader
{

    [XmlElement(ElementName = "SupplierOrderID")]
    public int? SupplierOrderID { get; set; }

    [XmlElement(ElementName = "CustomerOrderID")]
    public string? CustomerOrderID { get; set; }

    [XmlElement(ElementName = "CustomerOrderRef")]
    public string? CustomerOrderRef { get; set; }

    [XmlElement(ElementName = "OrderDate")]
    public OrderDate? OrderDate { get; set; }

    [XmlElement(ElementName = "OrderedBy")]
    public string? OrderedBy { get; set; }

    [XmlElement(ElementName = "DeliveryLocation")]
    public DeliveryLocation? DeliveryLocation { get; set; }

    [XmlElement(ElementName = "Comments")]
    public List<string>? Comments { get; set; }
}

[XmlRoot(ElementName = "OrderLine")]
public class OrderLine
{
    public OrderLine() { }

    public OrderLine(Dictionary<string, object> orderLine)
    {
        Material = new((string) orderLine["number"]);
        OrderedQuantity = (int) orderLine["aantal"];
    }

    [XmlElement(ElementName = "Material")]
    public Material? Material { get; set; }

    [XmlElement(ElementName = "OrderedQuantity")]
    public int? OrderedQuantity { get; set; }

    [XmlElement(ElementName = "SupplierOrderLineID")]
    public int? SupplierOrderLineID { get; set; }

    [XmlElement(ElementName = "CustomerOrderLineID")]
    public int? CustomerOrderLineID { get; set; }

    [XmlElement(ElementName = "UnitPrice")]
    public UnitPrice? UnitPrice { get; set; }

    [XmlElement(ElementName = "RequestedDeliveryDate")]
    public RequestedDeliveryDate? RequestedDeliveryDate { get; set; }

    [XmlElement(ElementName = "BOQuantity")]
    public int? BOQuantity { get; set; }

    [XmlElement(ElementName = "BOEstimatedDeliveryDate")]
    public BOEstimatedDeliveryDate? BOEstimatedDeliveryDate { get; set; }

    [XmlElement(ElementName = "DeliveryQuantity")]
    public int? DeliveryQuantity { get; set; }

    [XmlElement(ElementName = "DeliveryDate")]
    public DeliveryDate? DeliveryDate { get; set; }
}

[XmlRoot(ElementName = "Create")]
public class Create
{
    public Create() { }
    public Create(List<Dictionary<string, object>> orderLines)
    {
        OrderLine = orderLines.Select(l => new OrderLine(l)).ToList();
    }

    [XmlElement(ElementName = "OrderLine")]
    public List<OrderLine>? OrderLine { get; set; }
}

[XmlRoot(ElementName = "Detail")]
public class Detail
{

    [XmlElement(ElementName = "OrderHeader")]
    public OrderHeader? OrderHeader { get; set; }

    [XmlElement(ElementName = "OrderLine")]
    public List<OrderLine>? OrderLine { get; set; }
}

[XmlRoot(ElementName = "Order")]
public class Order
{
    public Order() { }

    public Order(List<Dictionary<string, object>> orderLines) {
        Create = new(orderLines);
    }

    [XmlElement(ElementName = "Create")]
    public Create? Create { get; set; }

    [XmlElement(ElementName = "Detail")]
    public Detail? Detail { get; set; }
}

[XmlRoot(ElementName = "Get")]
public class Get
{
    [XmlElement(ElementName = "Material")]
    public List<Material>? Material { get; set; }
}

[XmlRoot(ElementName = "Item")]
public class Item
{

    [XmlElement(ElementName = "Material")]
    public Material? Material { get; set; }

    [XmlElement(ElementName = "StockCode")]
    public string? StockCode { get; set; }

    [XmlElement(ElementName = "Stock")]
    public int? Stock { get; set; }

    [XmlElement(ElementName = "UnitOfMeasure")]
    public string? UnitOfMeasure { get; set; }

    [XmlElement(ElementName = "UnitPrice")]
    public UnitPrice? UnitPrice { get; set; }

    [XmlElement(ElementName = "SalesPackQuantity")]
    public int? SalesPackQuantity { get; set; }

    [XmlElement(ElementName = "PromotionCode")]
    public string? PromotionCode { get; set; }
}

[XmlRoot(ElementName = "SearchKeywords")]
public class SearchKeywords
{

    [XmlElement(ElementName = "Keyword")]
    public List<string>? Keyword { get; set; }
}

[XmlRoot(ElementName = "BrandKeywords")]
public class BrandKeywords
{

    [XmlElement(ElementName = "Keyword")]
    public List<string>? Keyword { get; set; }
}

[XmlRoot(ElementName = "List")]
public class List
{

    [XmlElement(ElementName = "NumberOfLines")]
    public int? NumberOfLines { get; set; }

    [XmlElement(ElementName = "Item")]
    public List<Item>? Item { get; set; }
}

[XmlRoot(ElementName = "Search")]
public class Search
{

    [XmlElement(ElementName = "SearchKeywords")]
    public SearchKeywords? SearchKeywords { get; set; }

    [XmlElement(ElementName = "BrandKeywords")]
    public BrandKeywords? BrandKeywords { get; set; }
}

[XmlRoot(ElementName = "Article")]
public class Article
{
    [XmlElement(ElementName = "Get")]
    public Get? Get { get; set; }

    [XmlElement(ElementName = "List")]
    public List? List { get; set; }

    [XmlElement(ElementName = "Search")]
    public Search? Search { get; set; }
}


[XmlRoot(ElementName = "Request")]
public class Request
{
    public Request() { }

    public Request(ConfigurationManager config)
    {
        Customer = new(config);
    }

    [XmlElement(ElementName = "Customer")]
    public Customer? Customer { get; set; }

    [XmlElement(ElementName = "ResponseType")]
    public string? ResponseType { get; set; }

    [XmlElement(ElementName = "Order")]
    public Order? Order { get; set; }

    [XmlElement(ElementName = "Article")]
    public Article? Article { get; set; }
}


[XmlRoot(ElementName = "Response")]
public class Response
{

    [XmlElement(ElementName = "Message")]
    public Message? Message { get; set; }

    [XmlElement(ElementName = "Article")]
    public Article? Article { get; set; }

    [XmlElement(ElementName = "Order")]
    public Order? Order { get; set; }
}


[XmlRoot(ElementName = "Message")]
public class Message
{

    [XmlAttribute(AttributeName = "code")]
    public int Code { get; set; }

    [XmlText]
    public string? Text { get; set; }
}

[XmlRoot(ElementName = "cebeoXML")]
public class CebeoXML
{

    public static CebeoXML CreateOrderRequest(List<Dictionary<string, object>> orderLines, ConfigurationManager config)
    {
        CebeoXML cebeoXML = new()
        {
            Request = new(config)
            {
                ResponseType = "Message",
                Order = new(orderLines)
            },
            Version = config["Cebeo:Version"],
            NoNamespaceSchemaLocation = config["Cebeo:NoNamespaceSchemaLocation"]
        };

        return cebeoXML;
    }

    public static CebeoXML CreateArticleRequest(string articleNumber, ConfigurationManager config)
    {
        CebeoXML cebeoXML = new()
        {
            Request = new(config)
            {
                ResponseType = "List",
                Article = new()
                {
                    Get = new()
                    {
                        Material = new Material[] {new() { SupplierItemID = articleNumber }}.ToList()
                    } 
                }
            },
            Version = config["Cebeo:Version"],
            NoNamespaceSchemaLocation = config["Cebeo:NoNamespaceSchemaLocation"]
        };

        return cebeoXML;
    }

    public static CebeoXML CreateArticleSearchRequest(string articleReference, ConfigurationManager config)
    {
        CebeoXML cebeoXML = new()
        {
            Request = new(config)
            {
                ResponseType = "List",
                Article = new()
                {
                    Search = new()
                    {
                        SearchKeywords = new()
                        {
                            Keyword = new string[] { articleReference }.ToList()
                        },
                        BrandKeywords = new()
                    }
                }
            },
            Version = config["Cebeo:Version"],
            NoNamespaceSchemaLocation = config["Cebeo:NoNamespaceSchemaLocation"]
        };

        return cebeoXML;
    }

    [XmlElement(ElementName = "Request")]
    public Request? Request { get; set; }

    [XmlElement(ElementName = "Response")]
    public Response? Response { get; set; }

    [XmlAttribute("noNamespaceWhatever", Namespace = XmlSchema.InstanceNamespace)]
    public string? NoNamespaceSchemaLocation { get; set; }

    [XmlAttribute(AttributeName = "version")]
    public string? Version { get; set; }

    //creates XML string format of current class object
    public string GetXML()
    {
        XmlSerializer serializer = new(typeof(CebeoXML));

        string xml = "";
        using (var sww = new StringWriter())
        {
            using (XmlWriter writer = XmlWriter.Create(sww))
            {
                serializer.Serialize(writer, this);
                xml = sww.ToString();
            }
        }

        return xml;
    }
}
