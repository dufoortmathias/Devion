namespace Api.Devion.Models;

public class MileageTimeChimp
{
    public int id { get; set; }
    public int custumerId { get; set; }
    public string? custumerName { get; set; }
    public int projectId { get; set; }
    public string? projectName { get; set; }
    public int? vehicleId { get; set; }
    public string? vehicleName { get; set; }
    public int userId { get; set; }
    public string? userDisplayName { get; set; }
    public DateTime date { get; set; }
    public string? fromAddress { get; set; }
    public string? toAddress { get; set; }
    public string? notes { get; set; }
    public double distance { get; set; }
    public bool isBilled { get; set; }
    public int type { get; set; }
    public int status { get; set; }
    public int statusIntern { get; set; }
    public int statusExtern { get; set; }
    public DateTime modified { get; set; }
}

public class MileageETS
{
    public int? PLA_ID { get; set; }
    public int? PLA_KM { get; set; }
    public string? PLA_PROJECT { get; set; }
    public string? PLA_SUBPROJECT { get; set; }
    public DateTime PLA_START { get; set; }
    public string? PLA_PERSOON { get; set; }
    public string? PLA_KM_DERDEN { get; set; }
    public string? PLA_KM_VERGOEDING { get; set; }

    public MileageETS() { }

    public MileageETS(MileageTimeChimp mileage, string projectNumber, string employeeNumber)
    {
        //TODO set length project_id string in env variables file
        PLA_KM = (int)Math.Ceiling(mileage.distance);
        string project = projectNumber;
        PLA_PROJECT = project[..Math.Min(project.Length, 7)];
        PLA_SUBPROJECT = project[7..];
        PLA_START = mileage.date.Date;
        PLA_PERSOON = employeeNumber[^4..];
        if (mileage.vehicleName == null)
        {
            throw new Exception($"In TimeChimp mileage with id \"{mileage.id}\" doesn't has a vehicle assigned");
        }
        PLA_KM_DERDEN = mileage.vehicleName[^4..];
        PLA_KM_VERGOEDING = "1";
    }
}