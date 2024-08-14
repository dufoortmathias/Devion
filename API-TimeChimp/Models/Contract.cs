namespace Api.Devion.Models;
public class Contract
{
    public int Id { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public float? WeekHours { get; set; }
    public float? HourlyRate { get; set; }
    public float? CostHourlyRate { get; set; }
    public ContractType? ContractType { get; set; }
}
public class ContractType
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

