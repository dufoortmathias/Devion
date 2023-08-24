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

[XmlRoot(ElementName = "OrderDate")]
public class OrderDate
{

    [XmlElement(ElementName = "Day")]
    public int Day { get; set; }

    [XmlElement(ElementName = "Month")]
    public int Month { get; set; }

    [XmlElement(ElementName = "Year")]
    public int Year { get; set; }
}

[XmlRoot(ElementName = "DeliveryAddress")]
public class DeliveryAddress
{

    [XmlElement(ElementName = "DeliverTo")]
    public string DeliverTo { get; set; }

    [XmlElement(ElementName = "Street")]
    public string Street { get; set; }

    [XmlElement(ElementName = "City")]
    public string City { get; set; }

    [XmlElement(ElementName = "PostalCode")]
    public int PostalCode { get; set; }
}

[XmlRoot(ElementName = "DeliveryLocation")]
public class DeliveryLocation
{

    [XmlElement(ElementName = "DeliveryAddress")]
    public DeliveryAddress DeliveryAddress { get; set; }

    [XmlElement(ElementName = "ContactPerson")]
    public string ContactPerson { get; set; }

    [XmlElement(ElementName = "ContactTelephone")]
    public string ContactTelephone { get; set; }
}

[XmlRoot(ElementName = "OrderHeader")]
public class OrderHeader
{

    [XmlElement(ElementName = "CustomerOrderID")]
    public string CustomerOrderID { get; set; }

    [XmlElement(ElementName = "CustomerOrderRef")]
    public string CustomerOrderRef { get; set; }

    [XmlElement(ElementName = "OrderDate")]
    public OrderDate OrderDate { get; set; }

    [XmlElement(ElementName = "DeliveryLocation")]
    public DeliveryLocation DeliveryLocation { get; set; }

    [XmlElement(ElementName = "PriceOnDeliveryNote")]
    public bool PriceOnDeliveryNote { get; set; }

    [XmlElement(ElementName = "DeliveryOption")]
    public string DeliveryOption { get; set; }

    [XmlElement(ElementName = "Comments")]
    public List<string> Comments { get; set; }
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

    [XmlElement(ElementName = "BrandCode")]
    public object BrandCode { get; set; }

    [XmlElement(ElementName = "Reference")]
    public object Reference { get; set; }

    [XmlElement(ElementName = "ReelCode")]
    public object ReelCode { get; set; }

    [XmlElement(ElementName = "ReelLength")]
    public object ReelLength { get; set; }

    [XmlElement(ElementName = "Description")]
    public object Description { get; set; }
}

[XmlRoot(ElementName = "RequestedDeliveryDate")]
public class RequestedDeliveryDate
{

    [XmlElement(ElementName = "Day")]
    public int Day { get; set; }

    [XmlElement(ElementName = "Month")]
    public int Month { get; set; }

    [XmlElement(ElementName = "Year")]
    public int Year { get; set; }
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

    [XmlElement(ElementName = "CustomerOrderLineID")]
    public object CustomerOrderLineID { get; set; }

    [XmlElement(ElementName = "Material")]
    public Material Material { get; set; }

    [XmlElement(ElementName = "OrderedQuantity")]
    public int OrderedQuantity { get; set; }

    [XmlElement(ElementName = "RequestedPrice")]
    public object RequestedPrice { get; set; }

    [XmlElement(ElementName = "RequestedDeliveryDate")]
    public RequestedDeliveryDate RequestedDeliveryDate { get; set; }
}

[XmlRoot(ElementName = "OrderAttribute")]
public class OrderAttribute
{

    [XmlAttribute(AttributeName = "name")]
    public string Name { get; set; }

    [XmlText]
    public string Text { get; set; }
}

[XmlRoot(ElementName = "Create")]
public class Create
{
    public Create() { }
    public Create(List<PurchaseOrderDetailETS> purchaseOrders)
    {
        OrderLine = purchaseOrders.Select(p => new OrderLine(p)).ToList();
    }

    [XmlElement(ElementName = "OrderHeader")]
    public OrderHeader OrderHeader { get; set; }

    [XmlElement(ElementName = "OrderLine")]
    public List<OrderLine> OrderLine { get; set; }

    [XmlElement(ElementName = "OrderAttribute")]
    public OrderAttribute OrderAttribute { get; set; }
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

[XmlRoot(ElementName = "Request")]
public class Request
{
    public Request() { }

    public Request(List<PurchaseOrderDetailETS> purchaseOrders, ConfigurationManager config)
    {
        Customer = new(config);
        ResponseType = "Message";
        Order = new(purchaseOrders);
    }

    [XmlElement(ElementName = "Customer")]
    public Customer Customer { get; set; }

    [XmlElement(ElementName = "ResponseType")]
    public string ResponseType { get; set; }

    [XmlElement(ElementName = "Order")]
    public Order Order { get; set; }
}

[XmlRoot(ElementName = "cebeoXML")]
public class CebeoXML
{
    public CebeoXML() { }

    public CebeoXML(List<PurchaseOrderDetailETS> purchaseOrders, ConfigurationManager config)
    {
        Request = new(purchaseOrders, config);
        Version = config["Cebeo:Version"];
    }

    [XmlElement(ElementName = "Request")]
    public Request Request { get; set; }

    [XmlAttribute(AttributeName = "version")]
    public string Version { get; set; }

    [XmlText]
    public string Text { get; set; }
}