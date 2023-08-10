namespace Api.Devion.Helpers.ETS;

public class ETSTimeHelper : ETSHelper
{
    private BearerTokenHttpClient TCClient;
    public ETSTimeHelper(FirebirdClientETS FBClient, BearerTokenHttpClient clientTC) : base(FBClient)
    {
        TCClient = clientTC;
        ETSClient = FBClient;
    }

    // get all times
    public List<timeETS> GetTime()
    {
        //get all times from ETS
        var response = ETSClient.selectQuery("select * from tbl_planning");
        //convert data to timeETS object
        List<timeETS> times = JsonTool.ConvertTo<List<timeETS>>(response);
        foreach (timeETS time in times)
        {
            //get data from ETS for the employee
            string query = $"select * from J2W_PNPX where PN_ID = {time.PLA_PERSOON}";
            string json = ETSClient.selectQuery(query);

            //convert data to naamTimeETS object
            List<naamTimeETS> naam = JsonTool.ConvertTo<List<naamTimeETS>>(json);
            //add the name to the time objects
            time.PN_NAM = naam.First().PN_NAM;
        }
        return times;
    }

    // add all times
    public List<timeETS> addTimesETS(List<timeETS> times)
    {
        foreach (timeETS time in times)
        {
            var response = ETSClient.insertQuery($"insert into tbl_planning (PLA_CAPTION, PLA_START, PLA_EINDE, PLA_KM_PAUZE, PLA_TEKST, PLA_PROJECT, PLA_SUBPROJECT, PLA_PERSOON) values ({time.PLA_CAPTION}, {time.PLA_START.Value.ToString("yyyy-MM-dd HH:mm:ss")}, {time.PLA_EINDE.Value.ToString("yyyy-MM-dd HH:mm:ss")}, {time.PLA_KM_PAUZE}, {time.PLA_TEKST}, {time.PLA_PROJECT}, {time.PLA_SUBPROJECT}, {time.PLA_PERSOON})");
        }
        return times;
    }

    //add a specific time
    public string addTime(timeETS time)
    {
        //get the max id from the table
        var response = ETSClient.selectQuery("select max(PLA_ID) from tbl_planning");

        List<maxValue> max = JsonTool.ConvertTo<List<maxValue>>(response);
        //set the id of the new time
        if (max[0].MAX == null)
        {
            time.PLA_ID = 1;
        }
        else
        {
            time.PLA_ID = max[0].MAX + 1;
        }

        //get data from ETS for the customer
        customerTimeChimp customer = new TimeChimpCustomerHelper(TCClient).GetCustomer(time.PLA_KLANT.ToString());
        time.PLA_KLANT = customer.relationId;

        //get data from ETS for the uurcode
        time.PLA_UURCODE = new TimeChimpUurcodeHelper(TCClient, ETSClient).GetUurcode(time.PLA_UURCODE).code.ToString();

        //get data from ETS for the employee
        time.PLA_PERSOON = new TimeChimpEmployeeHelper(TCClient).GetEmployee(Int32.Parse(time.PLA_PERSOON)).employeeNumber.ToString();

        //get data from ETS for the project and split the id in project and subproject
        var projectId = new TimeChimpProjectHelper(TCClient).GetProject(Int32.Parse(time.PLA_PROJECT)).code;
        time.PLA_SUBPROJECT = projectId.Substring(projectId.Length - 4);
        time.PLA_PROJECT = projectId.Substring(0, projectId.Length - 4);

        //create the caption
        time.PLA_CAPTION = "Proj:" + time.PLA_PROJECT + "/ " + time.PLA_SUBPROJECT;

        //get time data from timechimp for the name of the project and the name of the employee
        timeTimeChimp timechimp = new TimeChimpTimeHelper(TCClient, ETSClient).GetTime(time.timechimpId.ToString());

        //get the project from ETS
        ProjectETS projectETS = new ETSProjectHelper(ETSClient).GetProject(time.PLA_PROJECT);

        //create the text
        time.PLA_TEKST = time.PLA_PROJECT + ":" + time.PLA_SUBPROJECT + "\n" + projectETS.PR_KROM + "\n" + timechimp.projectName + "\n" + timechimp.userDisplayName + ": " + "\nWerkbon:";

        //create the query
        var query = $"INSERT INTO tbl_planning (PLA_ID, PLA_KLEUR, PLA_CAPTION, PLA_START, PLA_EINDE, PLA_KM_PAUZE, PLA_TEKST, PLA_PROJECT, PLA_SUBPROJECT, PLA_PERSOON, PLA_KLANT, PLA_UURCODE) " +
                    $"VALUES ({time.PLA_ID}, {time.PLA_KLEUR}, '{time.PLA_CAPTION}', '{time.PLA_START.Value.ToString("yyyy-MM-dd HH:mm:ss")}', " +
                    $"'{time.PLA_EINDE.Value.ToString("yyyy-MM-dd HH:mm:ss")}', '{time.PLA_KM_PAUZE}', '{time.PLA_TEKST}', " +
                    $"'{time.PLA_PROJECT}', '{time.PLA_SUBPROJECT}', '{time.PLA_PERSOON}', '{time.PLA_KLANT}', '{time.PLA_UURCODE}')";
        Console.WriteLine(query);

        //send data to ETS
        response = ETSClient.insertQuery(query);
        return response;
    }

    //add all times fomr last week
    public List<timeETS> addTimes()
    {
        //get all times from timechimp
        List<timeETS> times = new TimeChimpTimeHelper(TCClient, ETSClient).GetTimesLastWeek();
        List<int> ids = new List<int>();
        foreach (timeETS time in times)
        {
            //add the time to ETS
            var response = addTime(time);
            //add id to list for the change of status
            ids.Add(time.timechimpId);
        }

        //change the status of the times
        changeRegistrationStatusTimeChimp status = new TimeChimpTimeHelper(TCClient, ETSClient).changeStatus(ids);
        return times;
    }
}