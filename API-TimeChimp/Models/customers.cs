namespace Api.Devion.Models;

public class customerTimeChimp
{
    public int? id { get; set; }
    public bool active { get; set; }
    public string relationId { get; set; }
    public string? name { get; set; }
    public string? address { get; set; }
    public string? postalCode { get; set; }
    public string? city { get; set; }
    public string? country { get; set; }
    public string? phone { get; set; }
    public string? email { get; set; }
    public string? website { get; set; }
    public int paymentPeriod { get; set; }
    public double? tax { get; set; }
    public double? hourlyRate { get; set; }
    public double? mileageRate { get; set; }
    public string? iban { get; set; }
    public string? bic { get; set; }
    public string? vatNumber { get; set; }
    public string? kvkNumber { get; set; }
    public string? invoiceAddress { get; set; }
    public string? invoicePostalCode { get; set; }
    public string? invoiceCity { get; set; }
    public string? invoiceCountry { get; set; }
    public string? clientId { get; set; }
    public int[]? tagIds { get; set; }
    public string[]? tagNames { get; set; }
    public int VatRateId { get; set; }
    public double VatRatePercentage { get; set; }
    public contactsTimeChimp[]? contacts { get; set; }
    public DateTime modified { get; set; }

    public customerTimeChimp() { }

    public customerTimeChimp(CustomersETS customerETS) {
        name = customerETS.KL_NAM;
        email = customerETS.KL_EMAIL;
        phone = customerETS.KL_TEL;
        address = customerETS.KL_STR;
        country = customerETS.KL_LND;
        city = customerETS.KL_WPL;
        postalCode = customerETS.KL_PNR;
        website = customerETS.KL_WEBPAGE;
        relationId = customerETS.KL_COD;
    }
}

public class CustomersETS
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

