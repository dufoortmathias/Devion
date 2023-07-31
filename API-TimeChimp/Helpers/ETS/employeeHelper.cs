namespace Api.Devion.Helpers.ETS;

public static class ETSEmployeeHelper
{
    public static List<EmployeeETS> GetEmployees()
    {
        //connection with ETS
        var client = new FirebirdClientETS();
        var response = client.selectQuery("select * from J2W_PNPX");
        List<EmployeeETS> employees = JsonConvert.DeserializeObject<List<EmployeeETS>>(response);
        return employees;
    }
}