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
}
