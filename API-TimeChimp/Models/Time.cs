namespace Api.Devion.Models;

public class TimeTimeChimp
{
    public class SubmitForApprovalsTimeChimp
    {
        public int[]? ids { get; set; }
        public string? message { get; set; }
        public string? baseUrl { get; set; }
        public int[]? contactPersonIds { get; set; }
    }

    public class TagTimeChimp
    {
        public int id { get; set; }
        public string? name { get; set; }
        public bool active { get; set; }
        public int type { get; set; }
    }

    public int id { get; set; }
    public int customerId { get; set; }
    public string? customerName { get; set; }
    public int projectId { get; set; }
    public string? projectName { get; set; }
    public int projectTaskId { get; set; }
    public int TaskId { get; set; }
    public string? taskName { get; set; }
    public int userId { get; set; }
    public string? userDisplayName { get; set; }
    public string[]? userTags { get; set; }
    public DateTime? date { get; set; }
    public double? hours { get; set; }
    public string? notes { get; set; }
    public string? startEnd { get; set; }
    public DateTime? start { get; set; }
    public DateTime? end { get; set; }
    public double? pause { get; set; }
    public string? externalName { get; set; }
    public string? externalUrl { get; set; }
    public int status { get; set; }
    public int statusIntern { get; set; }
    public int statusExtern { get; set; }
    public TagTimeChimp[]? tags { get; set; }
    public DateTime modified { get; set; }
    public SubmitForApprovalsTimeChimp[]? submitForApprovals { get; set; }
}

public class TimeETS
{
    public int? timechimpStatus { get; set; }
    public int timechimpId { get; set; }
    public string? PLA_CAPTION { get; set; }
    public DateTime? PLA_START { get; set; }
    public DateTime? PLA_EINDE { get; set; }
    public string? PLA_KM_PAUZE { get; set; }
    public string? PLA_TEKST { get; set; }
    public string? PLA_PROJECT { get; set; }
    public string? PLA_SUBPROJECT { get; set; }
    public string? PLA_PERSOON { get; set; }
    public string? PN_NAM { get; set; }
    public int? PLA_ID { get; set; }
    public int? PLA_KLEUR { get; set; }
    public string? PLA_KLANT { get; set; }
    public string? PLA_UURCODE { get; set; }

    //constructor without specific parameters
    public TimeETS()
    { }

    //constructor to from timechimp class to ets class
    public TimeETS(TimeTimeChimp time)
    {
        timechimpStatus = time.status;
        timechimpId = time.id;
        PLA_START = time.start?.ToLocalTime();
        PLA_EINDE = time.end?.ToLocalTime();
        DateTime baseDateTime = DateTime.Parse("1899-12-30T00:00:00");
        PLA_KM_PAUZE = time.pause == null ? baseDateTime.ToString("yyyy-MM-dd HH:mm:ss") : baseDateTime.AddHours(time.pause.Value).ToString("yyyy-MM-dd HH:mm:ss");
        PN_NAM = time.userDisplayName;
        PLA_KLEUR = 12971235;
    }
}