namespace Api.Devion.Models;

public class UurcodeTimeChimp
{
    public int? id { get; set; }
    public bool? active { get; set; }
    public string? name { get; set; }
    public string? code { get; set; }
    public string? hourlyRate { get; set; }
    public bool? billable { get; set; }
    public bool? common { get; set; }
    public bool? unspecified { get; set; }
    public int[]? tagIds { get; set; }
    public string[]? tagNames { get; set; }

    //constructor without specific parameters
    public UurcodeTimeChimp() { }

    //constructor to from ets class to timechimp class
    public UurcodeTimeChimp(UurcodeETS uurcode)
    {
        name = uurcode.UR_OMS;
        code = uurcode.UR_COD;
        billable = true;
        common = false;
        unspecified = false;
        active = true;
        tagIds = new int[] { };
        tagNames = new string[] { };
        hourlyRate = null;
    }
}

public class UurcodeETS
{
    public string? UR_COD { get; set; }
    public string? UR_OMS { get; set; }
}