namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpMileageHelper : TimeChimpHelper
{
    public TimeChimpMileageHelper(WebClient client) : base(client)
    {
    }

    //get mileage
    public MileageTimeChimp GetMileage(int mileageId)
    {
        string enpoint = $"mileages/{mileageId}";

        //get data from timechimp
        string response = TCClient.GetAsync(enpoint);

        //convert data to mileageTimeChimp object
        MileageTimeChimp mileage = JsonTool.ConvertTo<ResponseTCMileage>(response).Result[0];
        return mileage;
    }

    //get mileages
    public List<MileageTimeChimp> GetMileages()
    {
        //get data from timechimp
        string response = TCClient.GetAsync("mileages");

        //convert data to mileageTimeChimp object
        List<MileageTimeChimp> mileages = JsonTool.ConvertTo<List<MileageTimeChimp>>(response);
        return mileages;
    }

    //get approved mileages by date
    public List<int> GetApprovedMileageIds()
    {
        string endpoint = $"mileages?$filter=status eq 'Approved'";

        //get data from timechimp between date and now
        string response = TCClient.GetAsync(endpoint);

        //convert data to mileageTimeChimp object
        List<MileageTimeChimp> mileages = JsonTool.ConvertTo<ResponseTCMileage>(response).Result.ToList();

        //return all mileages with status approved (2)
        return mileages.Select(mileages => mileages.Id).ToList();
    }

    //change status of mileage
    public MileageTimeChimp changeStatus(int mileageId)
    {
        //create new object
        var obj = new
        {
            status = TimeChimpStatus.Invoiced,
            mileages = new[] { new { id = mileageId } }
        };

        //send data to timechimp
        TCClient.PutAsync("mileages/status", JsonTool.ConvertFrom(obj));

        return new TimeChimpMileageHelper(TCClient).GetMileage(mileageId);
    }
}