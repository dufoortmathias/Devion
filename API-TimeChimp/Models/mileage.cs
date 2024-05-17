namespace Api.Devion.Models;

public enum MileageTypeTC
{
    Private,
    Business,
    HomeWork
}
public class MileageTimeChimp
{
    public int Id { get; set; }
    public DateTime? Date { get; set; }
    public string? FromAddress { get; set; }
    public string? ToAddress { get; set; }
    public string? Notes { get; set; }
    public float Distance { get; set; }
    public bool Billable { get; set; }
    public MileageTypeTC? Type { get; set; }
    public TimeChimpStatus? Status { get; set; }
    public TimeChimpStatus? ClientStatus { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }
    public CustomerTimeChimp Customer { get; set; }
    public ProjectTimeChimp Project { get; set; }
    public VehicleTimeChimp? Vehicle { get; set; }
    public EmployeeTimeChimp User { get; set; }
}

public class MileageETS
{
    public int? PLA_ID { get; set; }
    public int? PLA_KM { get; set; }
    public string? PLA_PROJECT { get; set; }
    public string? PLA_SUBPROJECT { get; set; }
    public DateTime? PLA_START { get; set; }
    public string? PLA_PERSOON { get; set; }
    public string? PLA_KM_DERDEN { get; set; }
    public string? PLA_KM_VERGOEDING { get; set; }

    public MileageETS() { }

    public MileageETS(MileageTimeChimp mileage, string projectNumber, string employeeNumber)
    {
        //TODO set length project_id string in env variables file
        PLA_KM = (int)Math.Ceiling(mileage.Distance);
        string project = projectNumber;
        PLA_PROJECT = project[..Math.Min(project.Length, 7)];
        PLA_SUBPROJECT = project[7..];
        PLA_START = mileage.Date;
        PLA_PERSOON = employeeNumber[^4..];
        PLA_KM_DERDEN = mileage.Vehicle.LicensePlate;
        PLA_KM_VERGOEDING = "1";
    }
}

public class VehicleTimeChimp
{
    public int Id { get; set; }
    public bool Active { get; set; }
    public string? Brand { get; set; }
    public string? Type { get; set; }
    public string? LicensePlate { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }
}

public class ResponseTCVehicle
{
    public VehicleTimeChimp[]? Result { get; set; }
    public int? Count { get; set; }
    public Link[]? Links { get; set; }
}

public class ResponseTCMileage
{
    public MileageTimeChimp[]? Result { get; set; }
    public int? Count { get; set; }
    public Link[]? Links { get; set; }
}