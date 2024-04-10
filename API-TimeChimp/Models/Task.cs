namespace Api.Devion.Models;

public class TaskTimechimp
{
    public int? Id { get; set; }
    public int? ProjectId { get; set; }
    public double? HourlyRate { get; set; }
    public double? BudgetHours { get; set; }
    public bool? Billable { get; set; }
    public bool? Unspecified { get; set; }
    public DateTime? Modified { get; set; }
    public DateTime? Created { get; set; }
    public List<Tag>? Tags { get; set; }
}

public class ProjectTaskETS
{
    public int VO_ID { get; set; }
    public string? VO_PROJ { get; set; }
    public string? VO_SUBPROJ { get; set; }
    public string? VO_UUR { get; set; }
    public double? VO_AANT { get; set; }
}