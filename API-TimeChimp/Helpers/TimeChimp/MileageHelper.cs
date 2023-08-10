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
        var response = TCClient.GetAsync("v1/mileage");

        //check if response is succesfull
        if (!response.IsCompletedSuccessfully)
        {
            throw new Exception("Error getting mileages from timechimp with endpoint: v1/mileage");
        }

        //convert data to mileageTimeChimp object
        List<mileageTimeChimp> mileages = JsonTool.ConvertTo<List<mileageTimeChimp>>(response.Result);
        return mileages;
    }

    //get mileages by date
    public List<mileageTimeChimp> GetMileagesByDate(DateTime date)
    {
        //get data from timechimp between date and now
        var response = TCClient.GetAsync($"v1/mileage/daterange/{date.ToString("yyyy-MM-dd")}/{DateTime.Now.Date.ToString("yyyy-MM-dd")}");

        //check if response is succesfull
        if (!response.IsCompletedSuccessfully)
        {
            throw new Exception($"Error getting mileages from timechimp with endpoint: v1/mileage/daterange/{date.ToString("yyyy-MM-dd")}/{DateTime.Now.Date.ToString("yyyy-MM-dd")}");
        }

        //convert data to mileageTimeChimp object
        List<mileageTimeChimp> mileages = JsonTool.ConvertTo<List<mileageTimeChimp>>(response.Result);

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

        //check if response is succesfull
        if (!response.IsCompletedSuccessfully)
        {
            throw new Exception("Error changing status of mileage in timechimp with endpoint: v1/mileage/changestatusintern");
        }

        return changes;
    }
}