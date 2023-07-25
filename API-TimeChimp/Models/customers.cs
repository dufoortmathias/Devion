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
}

