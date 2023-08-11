namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpTimeHelper : TimeChimpHelper
{
    private FirebirdClientETS ETSClient;

    public TimeChimpTimeHelper(BearerTokenHttpClient clientTC, FirebirdClientETS clientETS) : base(clientTC)
    {
        ETSClient = clientETS;
    }

    //get all times from the last week
    public List<timeETS> GetTimesLastWeek()
    {
        // get date from today and 7 days ago
        DateOnly today = DateOnly.FromDateTime(DateTime.Now);
        DateOnly lastWeek = DateOnly.FromDateTime(DateTime.Now.AddDays(-7));

        //check if dates are empty
        if (today == null || lastWeek == null)
        {
            throw new Exception("Error getting dates for times last week");
        }

        //get data from timechimp
        String response = TCClient.GetAsync($"v1/time/daterange/{lastWeek.ToString("yyyy-MM-dd")}/{today.ToString("yyyy-MM-dd")}");

        //convert data to timeTimeChimp object
        List<timeTimeChimp> times = JsonTool.ConvertTo<List<timeTimeChimp>>(response);

        List<timeETS> timesETS = times.Select(time => new timeETS(time)).ToList();
        List<timeETS> timesETSFiltered = new List<timeETS>();
        foreach (timeETS time in timesETS)
        {
            //check if status is approved (2)
            if (time.timechimpStatus == 2)
            {
                // get project code
                response = TCClient.GetAsync($"v1/projects/{time.PLA_PROJECT}");

                //convert data to projectTimeChimp object
                ProjectTimeChimp project = JsonTool.ConvertTo<ProjectTimeChimp>(response);

                // split project code
                string code = project.code;
                if (code != null && code.Length > 5)
                {

                    string projectCode = code.Substring(0, Math.Min(code.Length, 7));
                    time.PLA_PROJECT = projectCode;
                    string subProjectCode = code.Substring(7, Math.Min(code.Length - 7, 4));
                    time.PLA_SUBPROJECT = subProjectCode;
                    time.PLA_CAPTION = "Proj:" + time.PLA_PROJECT + "/ " + time.PLA_SUBPROJECT;
                    timeTimeChimp timechimp = times.Find(chimp => chimp.id == time.timechimpId);
                    ProjectETS projectETS = new ETSProjectHelper(ETSClient).GetProject(time.PLA_PROJECT);
                    time.PLA_TEKST = time.PLA_PROJECT + ":" + time.PLA_SUBPROJECT + "\n" + projectETS.PR_KROM + "\n" + timechimp.projectName + "\n" + timechimp.userDisplayName + ": " + "\nWerkbon:";

                }

                //get uurcode
                uurcodesTimeChimp uurcode = new TimeChimpUurcodeHelper(TCClient, ETSClient).GetUurcode(time.PLA_UURCODE);
                time.PLA_UURCODE = uurcode.code;

                //get personeelsnummer
                response = TCClient.GetAsync($"v1/users/{time.PLA_PERSOON}");

                //convert data to employeeTimeChimp object
                EmployeeTimeChimp user = JsonTool.ConvertTo<EmployeeTimeChimp>(response);
                time.PLA_PERSOON = user.employeeNumber;
                timesETSFiltered.Add(time);
            }
        }

        return timesETSFiltered;
    }

    //get timeids between 2 dates and status approved
    public String[] GetTimes(DateTime date)
    {
        //get data from timechimp
        String response = TCClient.GetAsync($"v1/time/daterange/{date.ToString("yyyy-MM-dd")}/{DateTime.Now.ToString("yyyy-MM-dd")}");

        //convert data to timeTimeChimp object
        List<timeTimeChimp> times = JsonTool.ConvertTo<List<timeTimeChimp>>(response);

        List<String> timeIds = new List<String>();
        foreach (timeTimeChimp time in times)
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
    public timeTimeChimp GetTime(string timeId)
    {
        //get data from timechimp
        String response = TCClient.GetAsync($"v1/time/{timeId}");

        //convert data to timeTimeChimp object
        timeTimeChimp time = JsonTool.ConvertTo<timeTimeChimp>(response);


        return time;
    }

    //change status of time
    public changeRegistrationStatusTimeChimp changeStatus(List<int> ids)
    {
        changeRegistrationStatusTimeChimp changes = new changeRegistrationStatusTimeChimp();
        changes.registrationIds = ids;
        changes.status = 3;

        //send data to timechimp
        String response = TCClient.PostAsync("v1/time/changestatusintern", JsonTool.ConvertFrom(changes));

        return changes;
    }
}