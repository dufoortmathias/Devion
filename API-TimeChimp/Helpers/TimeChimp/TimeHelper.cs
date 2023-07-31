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
        Console.WriteLine(lastWeek.ToString("yyyy-MM-dd") + " " + today.ToString("yyyy-MM-dd"));

        //get data from timechimp
        var response = client.GetAsync($"time/daterange/{lastWeek.ToString("yyyy-MM-dd")}/{today.ToString("yyyy-MM-dd")}");
        Console.WriteLine(response.Result);
        //convert data to timeTimeChimp object
        List<timeTimeChimp> times = JsonTool.ConvertTo<List<timeTimeChimp>>(response.Result);

        List<timeETS> timesETS = times.Select(time => new timeETS(time)).ToList();
        List<timeETS> timesETSFiltered = new List<timeETS>();
        foreach (timeETS time in timesETS)
        {
            if (time.timechimpStatus == 2)
            {
                // get project code
                response = client.GetAsync($"projects/{time.PLA_PROJECT}");
                ProjectTimeChimp project = JsonTool.ConvertTo<ProjectTimeChimp>(response.Result);

                // split project code
                string code = project.code;
                if (code != null && code.Length > 5)
                {
                    string projectCode = code.Substring(0, Math.Min(code.Length, 7));
                    time.PLA_PROJECT = projectCode;
                    string subProjectCode = code.Substring(7, Math.Min(code.Length - 7, 4));
                    time.PLA_SUBPROJECT = subProjectCode;
                    time.PLA_CAPTION = "Proj: " + time.PLA_PROJECT + "/" + time.PLA_SUBPROJECT;
                    timeTimeChimp timechimp = times.Find(chimp => chimp.id == time.timechimpId);
                    ProjectETS projectETS = ETSProjectHelper.GetProject(time.PLA_PROJECT);
                    time.PLA_TEKST = time.PLA_PROJECT + ":" + time.PLA_SUBPROJECT + "\n" + projectETS.PR_KROM + "\n" + timechimp.projectName + "\n" + timechimp.userDisplayName + ":" + "\nWerkbon:";

                }
                //get personeelsnummer
                response = client.GetAsync($"users/{time.PLA_PERSOON}");
                EmployeeTimeChimp user = JsonTool.ConvertTo<EmployeeTimeChimp>(response.Result);
                Console.WriteLine(user.employeeNumber);
                time.PLA_PERSOON = user.employeeNumber;
                timesETSFiltered.Add(time);
            }
            Console.WriteLine(time.PLA_PERSOON);
        }

        return timesETSFiltered;
    }

    public static changeRegistrationStatusTimeChimp changeStatus(List<int> ids)
    {
        var client = new BearerTokenHttpClient();
        changeRegistrationStatusTimeChimp changes = new changeRegistrationStatusTimeChimp();
        changes.registrationIds = ids;
        changes.status = 3;
        var response = client.PostAsync("time/changestatusintern", JsonTool.ConvertFrom(changes));
        return changes;
    }
}