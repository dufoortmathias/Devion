namespace Api.Devion.Models;

public class timeTimeChimp
{
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
    public DateTime date { get; set; }
    public Double? hours { get; set; }
    public string? notes { get; set; }
    public string? startEnd { get; set; }
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    public Double? pause { get; set; }
    public string? externalName { get; set; }
    public string? externalUrl { get; set; }
    public int status { get; set; }
    public int statusIntern { get; set; }
    public int statusExtern { get; set; }
    public tagTimeChimp[]? tags { get; set; }
    public DateTime modified { get; set; }
    public SubmitForApprovalsTimeChimp[]? submitForApprovals { get; set; }
}

public class timeETS
{
    public int timechimpStatus { get; set; }
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

    public timeETS() { }

    public timeETS(timeTimeChimp time)
    {
        this.timechimpStatus = time.status;
        this.timechimpId = time.id;
        this.PLA_CAPTION = time.taskName;
        this.PLA_START = time.start;
        this.PLA_EINDE = time.end;
        DateTime baseDateTime = DateTime.Parse("1899-12-30T00:00:00");
        this.PLA_KM_PAUZE = baseDateTime.AddMinutes((double)time.pause).ToString("yyyy-MM-ddTHH:mm:ss");
        this.PLA_TEKST = time.notes;
        this.PLA_PROJECT = time.projectId.ToString();
        this.PLA_SUBPROJECT = time.projectTaskId.ToString();
        this.PN_NAM = time.userDisplayName;
        this.PLA_PERSOON = time.userId.ToString();
    }
}

public class naamTimeETS
{
    public string? PN_NAM { get; set; }
}