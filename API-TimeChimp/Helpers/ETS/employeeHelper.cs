namespace Api.Devion.Helpers.ETS;

public class ETSEmployeeHelper : ETSHelper
{
    public ETSEmployeeHelper(FirebirdClientETS FBClient) : base(FBClient)
    {
    }

    //get all employees
    public List<EmployeeETS> GetEmployees()
    {
        //create query
        string query = "SELECT * FROM J2W_PNPX";

        //get data form ETS
        string response = ETSClient.selectQuery(query);

        //check if response is succesfull
        if (response == null)
        {
            throw new Exception("Error getting employees from ETS with query: " + query);
        }

        //convert data to employeeETS object
        List<EmployeeETS> employees = JsonTool.ConvertTo<List<EmployeeETS>>(response);
        return employees;
    }

    //get all employeeids that are changed after the given date
    public List<string> GetEmployeeIdsChangedAfter(DateTime date, string teamName)
    {
        //create query
        string query =
            $"SELECT J2W_PNPX.PN_ID " +
            $"FROM J2W_PNPX " +
            $"INNER JOIN TBL_PERSONEEL_PLOEG ON TBL_PERSONEEL_PLOEG.PPL_ID = J2W_PNPX.PN_PERSONEEL_PLOEG_ID " +
            $"WHERE J2W_PNPX.DATE_CHANGED >= '{date:MM/dd/yyyy HH:mm}' AND TBL_PERSONEEL_PLOEG.PPL_OMSCHRIJVING = '{teamName}'";

        //get data from ETS
        string json = ETSClient.selectQuery(query);

        //check if json is not empty
        if (json == null)
        {
            throw new Exception("Error getting employeeids from ETS with query: " + query);
        }

        //get all ids from json
        List<string> ids = JsonTool.ConvertTo<List<EmployeeETS>>(json)
            .Select(employee => employee.PN_ID)
            .ToList();
        return ids;
    }

    //get employee by employeeId
    public EmployeeETS GetEmployee(string employeeId)
    {
        //create query
        string query = $"select * from J2W_PNPX where PN_ID = {employeeId}";

        //get data from ETS
        string json = ETSClient.selectQuery(query);

        //check if json is not empty
        if (json == null)
        {
            throw new Exception("Error getting employee from ETS with query: " + query);
        }

        //convert data to employeeETS object
        EmployeeETS employee = JsonTool.ConvertTo<EmployeeETS[]>(json).FirstOrDefault();
        return employee;
    }
}