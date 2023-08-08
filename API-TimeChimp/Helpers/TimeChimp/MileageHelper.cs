namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpMileageHelper : TimeChimpHelper
{
    public TimeChimpMileageHelper(BearerTokenHttpClient client) : base(client)
    {
    }

    public List<mileageTimeChimp> GetMileages()
    {
        var response = TCClient.GetAsync("v1/mileage").Result;
        List<mileageTimeChimp> mileages = JsonTool.ConvertTo<List<mileageTimeChimp>>(response);
        return mileages;
    }

    public List<mileageTimeChimp> GetMileagesByDate(DateTime date)
    {
        var response = TCClient.GetAsync($"v1/mileage/daterange/{date.ToString("yyyy-MM-dd")}/{DateTime.Now.Date.ToString("yyyy-MM-dd")}").Result;
        List<mileageTimeChimp> mileages = JsonTool.ConvertTo<List<mileageTimeChimp>>(response);
        return mileages.FindAll(mileage => mileage.statusIntern == 2);
    }

    public changeRegistrationStatusTimeChimp changeStatus(List<int> ids)
    {
        changeRegistrationStatusTimeChimp changes = new changeRegistrationStatusTimeChimp();
        changes.registrationIds = ids;
        changes.status = 3;
        var response = TCClient.PostAsync("v1/mileage/changestatusintern", JsonTool.ConvertFrom(changes));
        return changes;
    }
}