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
    public Double hours { get; set; }
    public string? notes { get; set; }
    public string? startEnd { get; set; }
    public DateTime start { get; set; }
    public DateTime end { get; set; }
    public Double pause { get; set; }
    public string? externalName { get; set; }
    public string? externalUrl { get; set; }
    public int status { get; set; }
    public int statusIntern { get; set; }
    public int statusExtern { get; set; }
    public tagTimeChimp[]? tags { get; set; }
    public DateTime modified { get; set; }
    public SubmitForApprovalsTimeChimp[]? submitForApprovals { get; set; }
}
