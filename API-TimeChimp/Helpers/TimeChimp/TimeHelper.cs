namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpTimeHelper : TimeChimpHelper
{
    private readonly FirebirdClientETS ETSClient;

    public TimeChimpTimeHelper(WebClient clientTC, FirebirdClientETS clientETS) : base(clientTC)
    {
        ETSClient = clientETS;
    }

    //get timeids between 2 dates and status approved
    public string[] GetTimes()
    {
        //get data from timechimp
        string response = TCClient.GetAsync($"times?$filter=status eq 'Approved'");

        //convert data to timeTimeChimp object
        List<TimeTimeChimp> times = JsonTool.ConvertTo<ResponseTCTime>(response).Result.ToList();

        List<string> timeIds = new();
        foreach (TimeTimeChimp time in times)
        {
            //check if status is approved (2)
            if (time.Status == TimeChimpStatus.Approved)
            {
                timeIds.Add(time.Id.ToString());
            }
        }
        timeIds.Reverse();
        return timeIds.ToArray();
    }

    //get specific time
    public TimeTimeChimp GetTime(int timeId)
    {
        //get data from timechimp
        string response = TCClient.GetAsync($"times/{timeId}");

        //convert data to timeTimeChimp object
        TimeTimeChimp time = JsonTool.ConvertTo<ResponseTCTime>(response).Result[0];


        return time;
    }

    //change status of time
    public TimeTimeChimp InvoiceTime(int timeId)
    {
        Patch patch = new()
        {
            Op = "replace",
            Path = "/status",
            Value = "Invoiced"
        };

        //send data to timechimp
        _ = TCClient.PatchAsync($"times/{timeId}", JsonTool.ConvertFrom(patch));

        return new TimeChimpTimeHelper(TCClient, ETSClient).GetTime(timeId);
    }
}