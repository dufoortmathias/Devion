namespace Api.Devion.Models;

public class contactsTimeChimp
{
    public string? name { get; set; }
    public string? jobTitle { get; set; }
    public string? email { get; set; }
    public string? phone { get; set; }
    public Boolean useForInvoicing { get; set; }
    public Boolean active { get; set; }
    public int? id { get; set; }
    public int[]? customerIds { get; set; }

    //constructor without specific parameters
    public contactsTimeChimp() { }

    //constructor to from ets class to timechimp class
    public contactsTimeChimp(contactsETS contactETS)
    {
        name = contactETS.CO_TAV;
        email = contactETS.CO_EMAIL;
        phone = contactETS.CO_GSM;
        jobTitle = contactETS.FUT_OMSCHRIJVING;
        customerIds = new int[] { int.Parse(contactETS.CO_KLCOD) };
    }
}

public class contactsETS
{
    public Int32? C_CODE { get; set; }
    public string? CO_KLCOD { get; set; }
    public string? CO_TAV { get; set; }
    public string? CO_TAV2 { get; set; }
    public string? CO_TEL { get; set; }
    public string? CO_FAX { get; set; }
    public string? CO_GSM { get; set; }
    public string? CO_EMAIL { get; set; }
    public string? CO_ACTIEF { get; set; }
    public string? FUT_OMSCHRIJVING { get; set; }
}
