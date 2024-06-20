namespace Api.Devion.Models;

public class CustomerTimeChimp
{
    public int Id { get; set; }
    public bool? Active { get; set; }
    public bool? Unspecified { get; set; }
    public bool? Intern { get; set; }
    public string? Name { get; set; }
    public string? RelationId { get; set; }
    public AddressTimechimp? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }
    public int PaymentPeriod { get; set; }
    public float? HourlyRate { get; set; }
    public float? MileageRate { get; set; }
    public string? Iban { get; set; }
    public string? Bic { get; set; }
    public string? VatNumber { get; set; }
    public string? KvkNumber { get; set; }
    public AddressTimechimp? InvoiceAddress { get; set; }
    public string? Notes { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }
    public VatRate? VatRate { get; set; }
    public Tag[]? Tags { get; set; }
    public ContactTimeChimp[]? Contacts { get; set; }

    //constructor without specific parameters
    public CustomerTimeChimp() { }

    //constructor to from ets class to timechimp class
    public CustomerTimeChimp(CustomerETS customerETS)
    {
        Name = customerETS.KL_NAM;
        Email = customerETS.KL_EMAIL;
        Phone = customerETS.KL_TEL;
        Address = new AddressTimechimp()
        {
            Address = customerETS.KL_STR,
            Country = customerETS.KL_LND,
            City = customerETS.KL_WPL,
            PostalCode = customerETS.KL_PNR
        };
        Website = customerETS.KL_WEBPAGE;
        RelationId = customerETS.KL_COD;
        Active = customerETS.KL_BOE == "K";
    }
}

public class CustomerETS
{
    public string? KL_COD { get; set; }
    public string? KL_NAM { get; set; }
    public string? KL_OPV { get; set; }
    public string? KL_STR { get; set; }
    public string? KL_PNR { get; set; }
    public string? KL_WPL { get; set; }
    public string? KL_LND { get; set; }
    public string? KL_TEL { get; set; }
    public string? KL_FAX { get; set; }
    public string? KL_TEX { get; set; }
    public string? KL_EMAIL { get; set; }
    public string? KL_WEBPAGE { get; set; }
    public string? KL_T { get; set; }
    public string? KL_BTW { get; set; }
    public string? KL_TYP { get; set; }
    public string? KL_BOE { get; set; }
    public string? KL_VRIJ1 { get; set; }
    public string? KL_MNT { get; set; }
    public string? BM_OMS { get; set; }
    public string? BVW_CODE { get; set; }
    public string? BEL_CODE { get; set; }
    public string? REK_CODE { get; set; }
    public string? KL_SLECHTBET { get; set; }
}

public class ResponseTCCustomer
{
    public CustomerTimeChimp[]? Result { get; set; }
    public Link[]? Links { get; set; }
    public int? Count { get; set; }
}