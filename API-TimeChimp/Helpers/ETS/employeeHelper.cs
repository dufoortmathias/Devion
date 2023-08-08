namespace Api.Devion.Helpers.ETS;

public class ETSEmployeeHelper : ETSHelper
{
    public ETSEmployeeHelper(FirebirdClientETS FBClient) : base(FBClient)
    {
    }

    public List<EmployeeETS> GetEmployees()
    {
        //connection with ETS
        var response = ETSClient.selectQuery("select * from J2W_PNPX");
        List<EmployeeETS> employees = JsonTool.ConvertTo<List<EmployeeETS>>(response);
        return employees;
    }

    public List<string> GetEmployeeIdsChangedAfter(DateTime date)
    {
        string query = $"SELECT PN_ID FROM J2W_PNPX WHERE DATE_CHANGED >= '{date.ToString("MM/dd/yyyy HH:mm")}' ";
        string json = ETSClient.selectQuery(query);
        List<string> ids = JsonTool.ConvertTo<List<EmployeeETS>>(json)
            .Select(employee => employee.PN_ID)
            .ToList();
        return ids;
    }

    public EmployeeETS GetEmployee(String employeeId)
    {
        string query = $"select * from J2W_PNPX where PN_ID = {employeeId}";
        String json = ETSClient.selectQuery(query);
        EmployeeETS employee = JsonTool.ConvertTo<EmployeeETS[]>(json).FirstOrDefault();
        return employee;
    }
}