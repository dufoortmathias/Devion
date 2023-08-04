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

    public ProjectUserTimechimp() { }

    public ProjectUserTimechimp(EmployeeTimeChimp employee, ProjectTimeChimp subproject)
    {
        projectId = subproject.id;
        userId = employee.id;
        // userDisplayName = employee.displayName;
        active = true;
        projectManager = false;
    }
}