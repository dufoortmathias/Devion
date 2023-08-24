using System.Reflection.PortableExecutable;
using System.Xml.Serialization;

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
    public string CustomerNumber { get; set; }

    [XmlElement(ElementName = "UserName")]
    public string UserName { get; set; }

    [XmlElement(ElementName = "Password")]
    public string Password { get; set; }
}

[XmlRoot(ElementName = "UnitPrice")]
public class UnitPrice
{

    [XmlElement(ElementName = "NetPrice")]
    public string NetPrice { get; set; }
}

[XmlRoot(ElementName = "Material")]
public class Material
{
    public Material() { }

    public Material(string artikelNummer)
    {
        SupplierItemID = artikelNummer;
    }

    [XmlElement(ElementName = "SupplierItemID")]
    public string SupplierItemID { get; set; }
}

[XmlRoot(ElementName = "OrderLine")]
public class OrderLine
{
    public OrderLine() { }

    public OrderLine(PurchaseOrderDetailETS purchaseOrder)
    {
        Material = new(purchaseOrder.FD_ARTNR);
        OrderedQuantity = (int) purchaseOrder.FD_AANTAL;
    }

    [XmlElement(ElementName = "Material")]
    public Material Material { get; set; }

    [XmlElement(ElementName = "OrderedQuantity")]
    public int OrderedQuantity { get; set; }
}

[XmlRoot(ElementName = "Create")]
public class Create
{
    public Create() { }
    public Create(List<PurchaseOrderDetailETS> purchaseOrders)
    {
        OrderLine = purchaseOrders.Select(p => new OrderLine(p)).ToList();
    }

    [XmlElement(ElementName = "OrderLine")]
    public List<OrderLine> OrderLine { get; set; }
}

[XmlRoot(ElementName = "Order")]
public class Order
{
    public Order() { }

    public Order(List<PurchaseOrderDetailETS> purchaseOrders) {
        Create = new(purchaseOrders);
    }

    [XmlElement(ElementName = "Create")]
    public Create Create { get; set; }
}

[XmlRoot(ElementName = "Get")]
public class Get
{
    public Get() { }

    public Get(string[] articleNumbers)
    {
        Material = articleNumbers.Select(a => new Material(a)).ToList();
    }

    [XmlElement(ElementName = "Material")]
    public List<Material> Material { get; set; }
}

[XmlRoot(ElementName = "Item")]
public class Item
{

    [XmlElement(ElementName = "Material")]
    public Material Material { get; set; }

    [XmlElement(ElementName = "StockCode")]
    public string StockCode { get; set; }

    [XmlElement(ElementName = "Stock")]
    public int Stock { get; set; }

    [XmlElement(ElementName = "UnitOfMeasure")]
    public string UnitOfMeasure { get; set; }

    [XmlElement(ElementName = "UnitPrice")]
    public UnitPrice UnitPrice { get; set; }

    [XmlElement(ElementName = "SalesPackQuantity")]
    public int SalesPackQuantity { get; set; }
}

[XmlRoot(ElementName = "List")]
public class List
{

    [XmlElement(ElementName = "NumberOfLines")]
    public int NumberOfLines { get; set; }

    [XmlElement(ElementName = "Item")]
    public List<Item> Item { get; set; }
}

[XmlRoot(ElementName = "Article")]
public class Article
{
    public Article() { }

    public Article(string[] articleNumbers)
    {
        Get = new(articleNumbers);
    }

    [XmlElement(ElementName = "Get")]
    public Get Get { get; set; }

    [XmlElement(ElementName = "List")]
    public List List { get; set; }
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
    public Customer Customer { get; set; }

    [XmlElement(ElementName = "ResponseType")]
    public string ResponseType { get; set; }

    [XmlElement(ElementName = "Order")]
    public Order Order { get; set; }

    [XmlElement(ElementName = "Article")]
    public Article Article { get; set; }
}


[XmlRoot(ElementName = "Response")]
public class Response
{

    [XmlElement(ElementName = "Message")]
    public Message Message { get; set; }

    [XmlElement(ElementName = "Article")]
    public Article Article { get; set; }
}


[XmlRoot(ElementName = "Message")]
public class Message
{

    [XmlAttribute(AttributeName = "code")]
    public int Code { get; set; }

    [XmlText]
    public string Text { get; set; }
}

[XmlRoot(ElementName = "cebeoXML")]
public class CebeoXML
{

    public static CebeoXML CreateOrderRequest(List<PurchaseOrderDetailETS> purchaseOrders, ConfigurationManager config)
    {
        CebeoXML cebeoXML = new()
        {
            Request = new(config)
            {
                ResponseType = "Message",
                Order = new(purchaseOrders)
            },
            Version = config["Cebeo:Version"]
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
                Article = new(new string[] { articleNumber })
            },
            Version = config["Cebeo:Version"]
        };

        return cebeoXML;
    }

    [XmlElement(ElementName = "Request")]
    public Request Request { get; set; }

    [XmlElement(ElementName = "Response")]
    public Response Response { get; set; }

    [XmlAttribute(AttributeName = "version")]
    public string Version { get; set; }

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