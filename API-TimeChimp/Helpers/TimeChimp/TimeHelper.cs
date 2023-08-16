namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpTimeHelper : TimeChimpHelper
{
    private readonly FirebirdClientETS ETSClient;

    public TimeChimpTimeHelper(BearerTokenHttpClient clientTC, FirebirdClientETS clientETS) : base(clientTC)
    {
        ETSClient = clientETS;
    }

    //get timeids between 2 dates and status approved
    public string[] GetTimes(DateTime date)
    {
        //get data from timechimp
        string response = TCClient.GetAsync($"v1/time/daterange/{date:yyyy-MM-dd}/{DateTime.Now:yyyy-MM-dd}");

        //convert data to timeTimeChimp object
        List<TimeTimeChimp> times = JsonTool.ConvertTo<List<TimeTimeChimp>>(response);

        List<string> timeIds = new();
        foreach (TimeTimeChimp time in times)
        {
            //check if status is approved (2)
            if (time.status == 2)
            {
                timeIds.Add(time.id.ToString());
            }
        }
        timeIds.Reverse();
        return timeIds.ToArray();
    }

    //get specific time
    public TimeTimeChimp GetTime(int timeId)
    {
        //get data from timechimp
        string response = TCClient.GetAsync($"v1/time/{timeId}");

        //convert data to timeTimeChimp object
        TimeTimeChimp time = JsonTool.ConvertTo<TimeTimeChimp>(response);


        return time;
    }

    //change status of time
    public TimeTimeChimp InvoiceTime(int timeId)
    {
        Dictionary<string, object> changes = new()
        {
            {"registrationIds", new int[] { timeId} },
            {"status", 3 }
        };

        //send data to timechimp
        _ = TCClient.PostAsync("v1/time/changestatusintern", JsonTool.ConvertFrom(changes));

        return new TimeChimpTimeHelper(TCClient, ETSClient).GetTime(timeId);
    }
}