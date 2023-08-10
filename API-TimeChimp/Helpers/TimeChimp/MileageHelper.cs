namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpMileageHelper : TimeChimpHelper
{
    public TimeChimpMileageHelper(BearerTokenHttpClient client) : base(client)
    {
    }

    //get mileages
    public List<mileageTimeChimp> GetMileages()
    {
        //get data from timechimp
        var response = TCClient.GetAsync("v1/mileage").Result;

        //convert data to mileageTimeChimp object
        List<mileageTimeChimp> mileages = JsonTool.ConvertTo<List<mileageTimeChimp>>(response);
        return mileages;
    }

    //get mileages by date
    public List<mileageTimeChimp> GetMileagesByDate(DateTime date)
    {
        //get data from timechimp between date and now
        var response = TCClient.GetAsync($"v1/mileage/daterange/{date.ToString("yyyy-MM-dd")}/{DateTime.Now.Date.ToString("yyyy-MM-dd")}").Result;

        //convert data to mileageTimeChimp object
        List<mileageTimeChimp> mileages = JsonTool.ConvertTo<List<mileageTimeChimp>>(response);

        //return all mileages with status approved (2)
        return mileages.FindAll(mileage => mileage.statusIntern == 2);
    }

    //change status of mileage
    public changeRegistrationStatusTimeChimp changeStatus(List<int> ids)
    {
        //create new object
        changeRegistrationStatusTimeChimp changes = new changeRegistrationStatusTimeChimp();

        //set properties
        changes.registrationIds = ids;
        changes.status = 3;

        //send data to timechimp
        var response = TCClient.PostAsync("v1/mileage/changestatusintern", JsonTool.ConvertFrom(changes));
        return changes;
    }
}