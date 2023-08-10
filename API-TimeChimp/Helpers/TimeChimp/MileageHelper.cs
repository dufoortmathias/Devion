namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpMileageHelper : TimeChimpHelper
{
    public TimeChimpMileageHelper(BearerTokenHttpClient client) : base(client)
    {
    }

    //get mileage
    public MileageTimeChimp GetMileage(Int32 mileageId)
    {
        String enpoint = $"v1/mileage/{mileageId}";

        //get data from timechimp
        String response = TCClient.GetAsync(enpoint);

        //convert data to mileageTimeChimp object
        MileageTimeChimp mileage = JsonTool.ConvertTo<MileageTimeChimp>(response);
        return mileage;
    }

    //get mileages
    public List<MileageTimeChimp> GetMileages()
    {
        //get data from timechimp
        String response = TCClient.GetAsync("v1/mileage");

        //convert data to mileageTimeChimp object
        List<MileageTimeChimp> mileages = JsonTool.ConvertTo<List<MileageTimeChimp>>(response);
        return mileages;
    }

    //get approved mileages by date
    public List<Int32> GetApprovedMileageIdsByDate(DateTime date)
    {
        String endpoint = $"v1/mileage/daterange/{date.ToString("yyyy-MM-dd")}/{DateTime.Now.Date.ToString("yyyy-MM-dd")}";

        //get data from timechimp between date and now
        String response = TCClient.GetAsync(endpoint);

        //convert data to mileageTimeChimp object
        List<MileageTimeChimp> mileages = JsonTool.ConvertTo<List<MileageTimeChimp>>(response);

        //return all mileages with status approved (2)
        return mileages
            .FindAll(mileage => mileage.statusIntern == 2)
            .Select(mileage => mileage.id)
            .ToList();
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
        String response = TCClient.PostAsync("v1/mileage/changestatusintern", JsonTool.ConvertFrom(changes));

        return changes;
    }
}