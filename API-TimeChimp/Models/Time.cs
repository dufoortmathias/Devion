namespace Api.Devion.Models;

public enum TimeChimpStatus
{
    Open,
    PendingApproval,
    Approved,
    Invoiced,
    WrittenOff,
    Rejected
}

public class TimeTimeChimp
{
    public int Id { get; set; }
    public string? Date { get; set; }
    public float Hours { get; set; }
    public float? Pause { get; set; }
    public string? Notes { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public string? Timezone { get; set; }
    public TimeChimpStatus? Status { get; set; }
    public TimeChimpStatus? ClientStatus { get; set; }
    public float? Price { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }
    public EmployeeTimeChimp? User { get; set; }
    public CustomerTimeChimp? Customer { get; set; }
    public ProjectTimeChimp? Project { get; set; }
    public TaskTimeChimp? Task { get; set; }
    public List<Tag>? Tags { get; set; }
}

public class TimeETS
{
    public TimeChimpStatus? TimechimpStatus { get; set; }
    public int TimechimpId { get; set; }
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
        TimechimpStatus = time.Status;
        TimechimpId = time.Id;
        PLA_START = time.Start?.ToLocalTime();
        PLA_EINDE = time.End?.ToLocalTime();
        DateTime baseDateTime = DateTime.Parse("1899-12-30T00:00:00");
        PLA_KM_PAUZE = time.Pause == null ? baseDateTime.ToString("yyyy-MM-dd HH:mm:ss") : baseDateTime.AddHours(time.Pause.Value).ToString("yyyy-MM-dd HH:mm:ss");
        PN_NAM = time.User.DisplayName;
        PLA_KLEUR = 12971235;
    }
}

public class ResponseTCTime
{
    public TimeTimeChimp[]? Result { get; set; }
    public Link[]? Links { get; set; }
    public int? Count { get; set; }
}