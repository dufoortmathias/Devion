namespace Api.Devion.Helpers.TimeChimp;

public static class TimeChimpTimeHelper
{
    public static List<timeETS> GetTimesLastWeek()
    {
        // connection with timechimp
        var client = new BearerTokenHttpClient();


        // get date from today and 7 days ago
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        DateOnly lastWeek = DateOnly.FromDateTime(DateTime.Now.AddDays(-7));

        //get data from timechimp
        var response = client.GetAsync($"time/daterange/{lastWeek.ToString("yyyy-MM-dd")}/{today.ToString("yyyy-MM-dd")}");

        //convert data to timeTimeChimp object
        List<timeTimeChimp> times = JsonConvert.DeserializeObject<List<timeTimeChimp>>(response.Result);

        List<timeETS> timesETS = times.Select(time => new timeETS(time)).ToList();
        List<timeETS> timesETSFiltered = new List<timeETS>();
        foreach (timeETS time in timesETS)
        {
            if (time.timechimpStatus == 2)
            {
                // get project code
                response = client.GetAsync($"projects/{time.PLA_PROJECT}");
                ProjectTimeChimp project = JsonConvert.DeserializeObject<ProjectTimeChimp>(response.Result);

                // split project code
                string code = project.code;
                if (code != null && code.Length > 5)
                {
                    string projectCode = code.Substring(0, Math.Min(code.Length, 5));
                    time.PLA_PROJECT = projectCode;
                    string subProjectCode = code.Substring(5, Math.Min(code.Length - 5, 5));
                    time.PLA_SUBPROJECT = subProjectCode;
                }
                //get personeelsnummer
                response = client.GetAsync($"users/{time.PLA_PERSOON}");
                EmployeeTimeChimp user = JsonConvert.DeserializeObject<EmployeeTimeChimp>(response.Result);
                time.PLA_PERSOON = user.employeeNumber;
                timesETSFiltered.Add(time);
            }
        }

        return timesETSFiltered;
    }
}