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
        //create query
        string query = "SELECT * FROM tbl_planning";

        //get all times from ETS
        var response = ETSClient.selectQuery(query);

        //check if response is succesfull
        if (response == null)
        {
            throw new Exception("Error getting times from ETS with query: " + query);
        }

        //convert data to timeETS object
        List<timeETS> times = JsonTool.ConvertTo<List<timeETS>>(response);
        foreach (timeETS time in times)
        {
            //get data from ETS for the employee
            query = $"select * from J2W_PNPX where PN_ID = {time.PLA_PERSOON}";
            string json = ETSClient.selectQuery(query);

            //check if json is not empty
            if (json == null)
            {
                throw new Exception("Error getting employee from ETS with query: " + query);
            }

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
            //create query
            var query = $"insert into tbl_planning (PLA_CAPTION, PLA_START, PLA_EINDE, PLA_KM_PAUZE, PLA_TEKST, PLA_PROJECT, PLA_SUBPROJECT, PLA_PERSOON, PLA_KM, PLA_KM_HEEN_TERUG, PLA_KM_VERGOEDING) values ({time.PLA_CAPTION}, {time.PLA_START.Value.ToString("yyyy-MM-dd HH:mm:ss")}, {time.PLA_EINDE.Value.ToString("yyyy-MM-dd HH:mm:ss")}, {time.PLA_KM_PAUZE}, {time.PLA_TEKST}, {time.PLA_PROJECT}, {time.PLA_SUBPROJECT}, {time.PLA_PERSOON}, 0, 0, 0)";
            var response = ETSClient.insertQuery(query);
            //check if response is succesfull
            if (response == null)
            {
                throw new Exception("Error adding times to ETS with query: " + query);
            }
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

        //check if customer is not empty
        if (customer == null)
        {
            throw new Exception("Error getting customer from TimeChimp with id: " + time.PLA_KLANT);
        }

        time.PLA_KLANT = customer.relationId;

        //get data from ETS for the uurcode
        time.PLA_UURCODE = new TimeChimpUurcodeHelper(TCClient, ETSClient).GetUurcode(time.PLA_UURCODE).code.ToString();

        //check if uurcode is not empty
        if (time.PLA_UURCODE == null)
        {
            throw new Exception("Error getting uurcode from TimeChimp with id: " + time.PLA_UURCODE);
        }

        //get data from ETS for the employee
        time.PLA_PERSOON = new TimeChimpEmployeeHelper(TCClient).GetEmployee(Int32.Parse(time.PLA_PERSOON)).employeeNumber.ToString();

        //checking if employee is not empty
        if (time.PLA_PERSOON == null)
        {
            throw new Exception("Error getting employee from TimeChimp with id: " + time.PLA_PERSOON);
        }

        //get data from ETS for the project and split the id in project and subproject
        var projectId = new TimeChimpProjectHelper(TCClient).GetProject(Int32.Parse(time.PLA_PROJECT)).code;

        //check if projectId is not empty
        if (projectId == null)
        {
            throw new Exception("Error getting project from TimeChimp with id: " + time.PLA_PROJECT);
        }
        time.PLA_SUBPROJECT = projectId.Substring(projectId.Length - 4);
        time.PLA_PROJECT = projectId.Substring(0, projectId.Length - 4);

        //create the caption
        time.PLA_CAPTION = "Proj:" + time.PLA_PROJECT + "/ " + time.PLA_SUBPROJECT;

        //get time data from timechimp for the name of the project and the name of the employee
        timeTimeChimp timechimp = new TimeChimpTimeHelper(TCClient, ETSClient).GetTime(time.timechimpId.ToString());

        if (timechimp == null)
        {
            throw new Exception("Error getting time from TimeChimp with id: " + time.timechimpId);
        }

        //get the project from ETS
        ProjectETS projectETS = new ETSProjectHelper(ETSClient).GetProject(time.PLA_PROJECT);

        //check if project is not empty
        if (projectETS == null)
        {
            throw new Exception("Error getting project from ETS with id: " + time.PLA_PROJECT);
        }

        //create the text
        time.PLA_TEKST = time.PLA_PROJECT + ":" + time.PLA_SUBPROJECT + "\n" + projectETS.PR_KROM + "\n" + timechimp.projectName + "\n(" + timechimp.userDisplayName + "): " + time.PLA_UURCODE + " - " + new ETSUurcodeHelper(ETSClient).GetUurcode(time.PLA_UURCODE).UR_OMS + "\nTimeChimp: " + timechimp.id;

        //create the query
        var query = $"INSERT INTO tbl_planning (PLA_ID, PLA_KLEUR, PLA_CAPTION, PLA_START, PLA_EINDE, PLA_KM_PAUZE, PLA_TEKST, PLA_PROJECT, PLA_SUBPROJECT, PLA_PERSOON, PLA_KLANT, PLA_UURCODE, PLA_KM, PLA_KM_HEEN_TERUG, PLA_KM_VERGOEDING) " +
                    $"VALUES ({time.PLA_ID}, {time.PLA_KLEUR}, '{time.PLA_CAPTION}', '{time.PLA_START.Value.ToString("yyyy-MM-dd HH:mm:ss")}', " +
                    $"'{time.PLA_EINDE.Value.ToString("yyyy-MM-dd HH:mm:ss")}', '{time.PLA_KM_PAUZE}', '{time.PLA_TEKST}', " +
                    $"'{time.PLA_PROJECT}', '{time.PLA_SUBPROJECT}', '{time.PLA_PERSOON}', '{time.PLA_KLANT}', '{time.PLA_UURCODE}', " +
                    $"0, 0, 0)";

        //send data to ETS
        response = ETSClient.insertQuery(query);

        //check if response is succcesfull
        if (response == null)
        {
            throw new Exception("Error adding time to ETS with query: " + query);
        }
        return response;
    }

    //add all times fomr last week
    public List<timeETS> addTimes()
    {
        //get all times from timechimp
        List<timeETS> times = new TimeChimpTimeHelper(TCClient, ETSClient).GetTimesLastWeek();

        //check if times is not empty
        if (times == null)
        {
            throw new Exception("Error getting times from TimeChimp");
        }

        List<int> ids = new List<int>();
        foreach (timeETS time in times)
        {
            //add the time to ETS
            var response = addTime(time);

            //check if response is succesfull
            if (response == null)
            {
                throw new Exception("Error adding time to ETS");
            }

            //add id to list for the change of status
            ids.Add(time.timechimpId);
        }

        //change the status of the times
        changeRegistrationStatusTimeChimp status = new TimeChimpTimeHelper(TCClient, ETSClient).changeStatus(ids);

        //check if status is succesfull
        if (status == null)
        {
            throw new Exception("Error changing status of times in TimeChimp");
        }

        return times;
    }
}