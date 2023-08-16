namespace Api.Devion.Models;

public class ProjectUserTimechimp
{
    public int? id { get; set; }
    public int? projectId { get; set; }
    public int? userId { get; set; }
    public string? userDisplayName { get; set; }
    public double? hourlyRate { get; set; }
    public double? budgetHours { get; set; }
    public bool? active { get; set; }
    public DateTime? modified { get; set; }
    public object[]? hourlyRates { get; set; }
    public bool? projectManager { get; set; }

    //constructor without specific parameters
    public ProjectUserTimechimp() { }

    //constructor to from timechimp class to ets class
    public ProjectUserTimechimp(int employeeId, int subprojectId)
    {
        projectId = subprojectId;
        userId = employeeId;
        // userDisplayName = employee.displayName;
        active = true;
        projectManager = false;
    }
}