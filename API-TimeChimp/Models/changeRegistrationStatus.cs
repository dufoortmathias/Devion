namespace Api.Devion.Models;

public class changeRegistrationStatusTimeChimp
{
    public List<int> registrationIds { get; set; }
    public int status { get; set; }
    public string? message { get; set; }
}
