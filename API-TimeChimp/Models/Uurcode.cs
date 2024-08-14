namespace Api.Devion.Models;

public class UurcodeTimeChimp
{
    public int? Id { get; set; }
    public bool? Active { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? HourlyRate { get; set; }
    public bool? Billable { get; set; }
    public bool? Common { get; set; }
    public bool? Unspecified { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }
    public Tag[]? Tags { get; set; }

    //constructor without specific parameters
    public UurcodeTimeChimp() { }

    //constructor to from ets class to timechimp class
    public UurcodeTimeChimp(UurcodeETS uurcode)
    {
        Name = uurcode.UR_OMS;
        Code = uurcode.UR_COD;
        Billable = true;
        Common = true;
        Unspecified = false;
        Active = true;
        Tags = Array.Empty<Tag>();
        HourlyRate = null;
    }
}

public class UurcodeETS
{
    public string? UR_COD { get; set; }
    public string? UR_OMS { get; set; }
}

public class ResponseTCUurcode
{
    public UurcodeTimeChimp[]? Result { get; set; }
    public int? Count { get; set; }
}