namespace Api.Devion.Helpers.TimeChimp;

public static class TimeChimpTimeHelper
{
    public static List<timeTimeChimp> GetTimesLastWeek()
    {
        // connection with timechimp
        var client = new BearerTokenHttpClient();


        // get date from today and 7 days ago
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        DateOnly lastWeek = DateOnly.FromDateTime(DateTime.Now.AddDays(-7));

        //get data from timechimp
        var response = client.GetAsync($"time/daterange/{lastWeek.ToString("yyyy-MM-dd")}/{today.ToString("yyyy-MM-dd")}");

        //convert data to timeTimeChimp object
        timeTimeChimp[] times = JsonConvert.DeserializeObject<timeTimeChimp[]>(response.Result);
        List<timeTimeChimp> goedgekeurdeUren = new List<timeTimeChimp>();
        changeRegistrationStatusTimeChimp changeRegistrationStatus = new changeRegistrationStatusTimeChimp();
        List<int> registrationIds = new List<int>();
        foreach (timeTimeChimp time in times)
        {
            //checking uren is goedgekeurd
            if (time.status == 2)
            {
                Console.WriteLine(time.id);
                var id = time.id;
                if (id == null)
                {
                    Console.WriteLine("id is null");
                }
                else
                {
                    registrationIds.Add(id);
                    goedgekeurdeUren.Add(time);
                }

            }
        }

        //put registrationid in ChangeRegistrationStatusTimeChimp object
        changeRegistrationStatus.registrationIds = registrationIds;
        changeRegistrationStatus.status = 3;
        changeRegistrationStatus.message = "gefactureerd";


        //send to timechimp
        client.PostAsync("time/changestatusintern", JsonConvert.SerializeObject(changeRegistrationStatus));
        //return data
        return goedgekeurdeUren;
    }
}