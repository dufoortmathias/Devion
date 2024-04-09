namespace Api.Devion.Models;

public class ProjectVoortgangTask
{
    public int Id { get; set; }
    public string? Active { get; set; }
    public string? TaskMode { get; set; }
    public string? Name { get; set; }
    public string? Duration { get; set; }
    public string? Start { get; set; }
    public string? Finish { get; set; }
    public string? Predecessors { get; set; }
    public int OutlineLevel { get; set; }
    public string? Notes { get; set; }
}

