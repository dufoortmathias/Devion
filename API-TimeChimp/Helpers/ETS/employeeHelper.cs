namespace Api.Devion.Helpers.ETS;

public static class ETSEmployeeHelper
{
    public static List<EmployeeETS> GetEmployees()
    {
        //connection with ETS
        var client = new FirebirdClientETS();
        var response = client.selectQuery("select * from J2W_PNPX");
        List<EmployeeETS> employees = JsonTool.ConvertTo<List<EmployeeETS>>(response);
        return employees;
    }

    public static List<string> GetEmployeeIdsChangedAfter(DateTime date)
    {
        FirebirdClientETS client = new FirebirdClientETS();
        string query = $"SELECT PN_ID FROM J2W_PNPX WHERE DATE_CHANGED >= '{date.ToString("MM/dd/yyyy HH:mm")}' ";
        string json = client.selectQuery(query);
        List<string> ids = JsonTool.ConvertTo<List<EmployeeETS>>(json)
            .Select(employee => employee.PN_ID)
            .ToList();
        return ids;
    }

    public static EmployeeETS GetEmployee(String employeeId)
    {
        FirebirdClientETS client = new();
        string query = $"select * from J2W_PNPX where PN_ID = {employeeId}";
        String json = client.selectQuery(query);
        EmployeeETS employee = JsonTool.ConvertTo<EmployeeETS[]>(json).FirstOrDefault();
        return employee;
    }
}