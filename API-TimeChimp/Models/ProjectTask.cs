namespace Api.Devion.Models;

public class ProjectTaskTimechimp
{
    public int? id { get; set; }
    public int? projectId { get; set; }
    public int? taskId { get; set; }
    public string? taskName { get; set; }
    public double? hourlyRate { get; set; }
    public double? budgetHours { get; set; }
    public bool? billable { get; set; }
    public bool? unspecified { get; set; }
    public DateTime? modified { get; set; }
}

public class ProjectTaskETS
{
    public string? VO_PROJ { get; set; }
    public string? VO_SUBPROJ { get; set; }
    public string? VO_UUR { get; set; }
    public double? VO_AANT { get; set; }
}