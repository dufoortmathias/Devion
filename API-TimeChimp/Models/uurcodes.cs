namespace Api.Devion.Models;

public class uurcodesTimeChimp
{
    public int id { get; set; }
    public string active { get; set; }
    public string name { get; set; }
    public string code { get; set; }
    public string hourlyRate { get; set; }
    public Boolean billable { get; set; }
    public Boolean common { get; set; }
    public Boolean unspecified { get; set; }
    public int[] tagIds { get; set; }
    public string[] tagNames { get; set; }

    public uurcodesTimeChimp() { }

    public uurcodesTimeChimp(uurcodesETS uurcode)
    {
        this.name = uurcode.UR_OMS;
        this.code = uurcode.UR_COD;
        this.billable = true;
        this.common = true;
        this.unspecified = false;
        this.active = "true";
        this.tagIds = new int[] { };
        this.tagNames = new string[] { };
        this.hourlyRate = null;
    }
}

public class uurcodesETS
{
    public string UR_COD { get; set; }
    public string UR_OMS { get; set; }
}