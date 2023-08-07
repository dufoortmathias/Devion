namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpMileageHelper
{
    public static List<mileageTimeChimp> GetMileages()
    {
        var client = new BearerTokenHttpClient();

        var response = client.GetAsync("mileage").Result;
        Console.WriteLine(response);
        List<mileageTimeChimp> mileages = JsonTool.ConvertTo<List<mileageTimeChimp>>(response);
        return mileages;
    }

    public static List<mileageTimeChimp> GetMileagesByDate(DateTime date)
    {
        var client = new BearerTokenHttpClient();

        var response = client.GetAsync($"mileage/daterange/{date.ToString("yyyy-MM-dd")}/{DateTime.Now.Date.ToString("yyyy-MM-dd")}").Result;
        List<mileageTimeChimp> mileages = JsonTool.ConvertTo<List<mileageTimeChimp>>(response);
        return mileages.FindAll(mileage => mileage.statusIntern == 2);
    }

    public static changeRegistrationStatusTimeChimp changeStatus(List<int> ids)
    {
        var client = new BearerTokenHttpClient();
        changeRegistrationStatusTimeChimp changes = new changeRegistrationStatusTimeChimp();
        changes.registrationIds = ids;
        changes.status = 3;
        var response = client.PostAsync("mileage/changestatusintern", JsonTool.ConvertFrom(changes));
        return changes;
    }
}