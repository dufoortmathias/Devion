namespace Api.Devion.Models;

public class ContactTimeChimp
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? JobTitle { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public bool? UseForInvoicing { get; set; }
    public bool? Active { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }
    public CustomerTimeChimp[]? Customers { get; set; }

    //constructor without specific parameters
    public ContactTimeChimp() { }

    //constructor to from ets class to timechimp class
    public ContactTimeChimp(ContactETS contactETS, int customerId)
    {
        if (string.IsNullOrEmpty(contactETS.CO_CONTACTPERSOON?.Trim()))
        {
            throw new Exception("Contact has no name");
        }
        Name = contactETS.CO_CONTACTPERSOON;
        Email = contactETS.CO_EMAIL;
        Phone = contactETS.CO_GSM;
        JobTitle = contactETS.FUT_OMSCHRIJVING;
        Customers = new CustomerTimeChimp[] { new() { Id = customerId } };
    }
}

public class ContactETS
{
    public int? C_CODE { get; set; }
    public string? CO_KLCOD { get; set; }
    public string? CO_TAV { get; set; }
    public string? CO_TAV2 { get; set; }
    public string? CO_TEL { get; set; }
    public string? CO_FAX { get; set; }
    public string? CO_GSM { get; set; }
    public string? CO_EMAIL { get; set; }
    public string? CO_ACTIEF { get; set; }
    public string? FUT_OMSCHRIJVING { get; set; }
    public string? CO_CONTACTPERSOON { get; set; }
}

public class ResponseTCContact
{
    public ContactTimeChimp[]? Result { get; set; }
    public Link[]? Links { get; set; }
    public int? Count { get; set; }
}