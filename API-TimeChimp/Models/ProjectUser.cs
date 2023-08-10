namespace Api.Devion.Models;

public class ProjectUserTimechimp
{
    public Int32? id { get; set; }
    public Int32? projectId { get; set; }
    public Int32? userId { get; set; }
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
    public ProjectUserTimechimp(Int32 employeeId, Int32 subprojectId)
    {
        projectId = subprojectId;
        userId = employeeId;
        // userDisplayName = employee.displayName;
        active = true;
        projectManager = false;
    }
}