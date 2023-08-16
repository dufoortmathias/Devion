namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpTimeHelper : TimeChimpHelper
{
    private FirebirdClientETS ETSClient;

    public TimeChimpTimeHelper(BearerTokenHttpClient clientTC, FirebirdClientETS clientETS) : base(clientTC)
    {
        ETSClient = clientETS;
    }

    //get timeids between 2 dates and status approved
    public String[] GetTimes(DateTime date)
    {
        //get data from timechimp
        String response = TCClient.GetAsync($"v1/time/daterange/{date.ToString("yyyy-MM-dd")}/{DateTime.Now.ToString("yyyy-MM-dd")}");

        //convert data to timeTimeChimp object
        List<TimeTimeChimp> times = JsonTool.ConvertTo<List<TimeTimeChimp>>(response);

        List<String> timeIds = new List<String>();
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
    public TimeTimeChimp GetTime(Int32 timeId)
    {
        //get data from timechimp
        String response = TCClient.GetAsync($"v1/time/{timeId}");

        //convert data to timeTimeChimp object
        TimeTimeChimp time = JsonTool.ConvertTo<TimeTimeChimp>(response);


        return time;
    }

    //change status of time
    public TimeTimeChimp InvoiceTime(Int32 timeId)
    {
        Dictionary<String, Object> changes = new()
        {
            {"registrationIds", new Int32[] { timeId} },
            {"status", 3 }
        };

        //send data to timechimp
        String response = TCClient.PostAsync("v1/time/changestatusintern", JsonTool.ConvertFrom(changes));

        return new TimeChimpTimeHelper(TCClient, ETSClient).GetTime(timeId);
    }
}