namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpMileageHelper : TimeChimpHelper
{
    public TimeChimpMileageHelper(WebClient client) : base(client)
    {
    }

    //get mileage
    public MileageTimeChimp GetMileage(int mileageId)
    {
        string enpoint = $"v1/mileage/{mileageId}";

        //get data from timechimp
        string response = TCClient.GetAsync(enpoint);

        //convert data to mileageTimeChimp object
        MileageTimeChimp mileage = JsonTool.ConvertTo<MileageTimeChimp>(response);
        return mileage;
    }

    //get mileages
    public List<MileageTimeChimp> GetMileages()
    {
        //get data from timechimp
        string response = TCClient.GetAsync("v1/mileage");

        //convert data to mileageTimeChimp object
        List<MileageTimeChimp> mileages = JsonTool.ConvertTo<List<MileageTimeChimp>>(response);
        return mileages;
    }

    //get approved mileages by date
    public List<int> GetApprovedMileageIdsByDate(DateTime date)
    {
        string endpoint = $"v1/mileage/daterange/{date:yyyy-MM-dd}/{DateTime.Now.Date:yyyy-MM-dd}";

        //get data from timechimp between date and now
        string response = TCClient.GetAsync(endpoint);

        //convert data to mileageTimeChimp object
        List<MileageTimeChimp> mileages = JsonTool.ConvertTo<List<MileageTimeChimp>>(response);

        //return all mileages with status approved (2)
        return mileages
            .FindAll(mileage => mileage.statusIntern == 2)
            .Select(mileage => mileage.id)
            .Reverse()
            .ToList();
    }

    //change status of mileage
    public MileageTimeChimp changeStatus(int mileageId)
    {
        //create new object
        Dictionary<string, object> changes = new()
        {
            {"registrationIds", new int[] { mileageId } },
            {"status", 3 }
        };

        //send data to timechimp
        TCClient.PostAsync("v1/mileage/changestatusintern", JsonTool.ConvertFrom(changes));

        return new TimeChimpMileageHelper(TCClient).GetMileage(mileageId);
    }
}