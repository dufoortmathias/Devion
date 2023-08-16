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
    public List<TimeETS> GetTime()
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
        List<TimeETS> times = JsonTool.ConvertTo<List<TimeETS>>(response);
        foreach (TimeETS time in times)
        {
            //get data from ETS for the employee
            query = $"select PN_NAM from J2W_PNPX where PN_ID = {time.PLA_PERSOON}";
            string json = ETSClient.selectQuery(query);

            //check if json is not empty
            if (json == null)
            {
                throw new Exception("Error getting employee from ETS with query: " + query);
            }

            //add the name to the time objects
            time.PN_NAM = JsonTool.ConvertTo<List<Dictionary<String, String>>>(json).First()["PN_NAM"];
        }
        return times;
    }

    //add a specific time
    public string AddTime(TimeTimeChimp timeTC)
    {
        TimeETS timeETS = new(timeTC);

        //get the max id from the table
        var response = ETSClient.selectQuery("select max(PLA_ID) from tbl_planning");

        Int32 max = JsonTool.ConvertTo<List<Dictionary<String, Int32>>>(response).First()["MAX"];
        //set the id of the new time
        timeETS.PLA_ID = max + 1;

        //get data from ETS for the customer
        customerTimeChimp customer = new TimeChimpCustomerHelper(TCClient).GetCustomer(timeTC.customerId) ?? throw new Exception("Error getting customer from TimeChimp with id: " + timeTC.customerId);
        timeETS.PLA_KLANT = customer.relationId;

        //get data from ETS for the uurcode
        uurcodesTimeChimp uurCode = new TimeChimpUurcodeHelper(TCClient, ETSClient).GetUurcode(timeTC.TaskId) ?? throw new Exception("Error getting uurcode from TimeChimp with id: " + timeTC.TaskId);
        timeETS.PLA_UURCODE = uurCode.code;

        //get data from ETS for the project
        ProjectTimeChimp subProject = new TimeChimpProjectHelper(TCClient).GetProject(timeTC.projectId) ?? throw new Exception("Error getting project from TimeChimp with id: " + timeTC.projectId);
        timeETS.PLA_PROJECT = subProject.code[..7];
        timeETS.PLA_SUBPROJECT = subProject.code[7..];

        //get data from ETS for the employee
        EmployeeTimeChimp employee = new TimeChimpEmployeeHelper(TCClient).GetEmployee(timeTC.userId) ?? throw new Exception("Error getting employee from TimeChimp with id: " + timeTC.userId);
        timeETS.PLA_PERSOON = employee.employeeNumber;

        //create the caption
        timeETS.PLA_CAPTION = subProject.code;

        //create the text
        timeETS.PLA_TEKST =
            $"{timeTC.projectName}: {uurCode.name} ({timeETS.PLA_UURCODE})\n" +
            $"TimeChimp: {timeTC.id}\n" +
            timeTC.notes;

        //create the query
        var query = $"INSERT INTO tbl_planning (PLA_ID, PLA_KLEUR, PLA_CAPTION, PLA_START, PLA_EINDE, PLA_KM_PAUZE, PLA_TEKST, PLA_PROJECT, PLA_SUBPROJECT, PLA_PERSOON, PLA_KLANT, PLA_UURCODE, PLA_KM, PLA_KM_HEEN_TERUG, PLA_KM_VERGOEDING) " +
                    $"VALUES ({timeETS.PLA_ID}, {timeETS.PLA_KLEUR}, '{timeETS.PLA_CAPTION}', '{timeETS.PLA_START.Value.ToString("yyyy-MM-dd HH:mm:ss")}', " +
                    $"'{timeETS.PLA_EINDE.Value.ToString("yyyy-MM-dd HH:mm:ss")}', '{timeETS.PLA_KM_PAUZE}', '{timeETS.PLA_TEKST}', " +
                    $"'{timeETS.PLA_PROJECT}', '{timeETS.PLA_SUBPROJECT}', '{timeETS.PLA_PERSOON}', '{timeETS.PLA_KLANT}', '{timeETS.PLA_UURCODE}', " +
                    $"0, 0, 0)";

        //send data to ETS
        ETSClient.insertQuery(query);

        //check if response is succcesfull
        if (response == null)
        {
            throw new Exception("Error adding time to ETS with query: " + query);
        }
        return response;
    }
}