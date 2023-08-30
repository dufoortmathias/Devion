namespace Api.Devion.Models;

public class ContactTimeChimp
{
    public string? name { get; set; }
    public string? jobTitle { get; set; }
    public string? email { get; set; }
    public string? phone { get; set; }
    public bool useForInvoicing { get; set; }
    public bool active { get; set; }
    public int? id { get; set; }
    public int[]? customerIds { get; set; }

    //constructor without specific parameters
    public ContactTimeChimp() { }

    //constructor to from ets class to timechimp class
    public ContactTimeChimp(ContactETS contactETS, int customerId)
    {
        if (contactETS.CO_TAV == null)
        {
            throw new Exception("Contact has no name");
        }
        name = contactETS.CO_CONTACTPERSOON;
        email = contactETS.CO_EMAIL;
        phone = contactETS.CO_GSM;
        jobTitle = contactETS.FUT_OMSCHRIJVING;
        customerIds = new int[] { customerId };
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
