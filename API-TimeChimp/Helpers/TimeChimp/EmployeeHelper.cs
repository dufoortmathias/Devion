namespace Api.Devion.Helpers.TimeChimp
{
    public static class TimeChimpEmployeeHelper
    {
        public static Boolean EmployeeExists(EmployeeETS employeeETS)
        {
            return GetEmployees().Any(employee => employee.userName != null && employee.userName.Equals(employeeETS.PN_EMAIL));
        }

        public static List<EmployeeTimeChimp> GetEmployees()
        {
            var client = new BearerTokenHttpClient();

            String response = client.GetAsync("users").Result;

            List<EmployeeTimeChimp> employees = JsonTool.ConvertTo<List<EmployeeTimeChimp>>(response);
            return employees;
        }

        public static EmployeeTimeChimp CreateEmployee(EmployeeTimeChimp employee)
        {
            var client = new BearerTokenHttpClient();

            employee.email = employee.userName;
            employee.sendInvitation = true;

            String response = client.PostAsync("users", JsonTool.ConvertFrom(employee)).Result;

            EmployeeTimeChimp employeeResponse = JsonTool.ConvertTo<EmployeeTimeChimp>(response);
            return employeeResponse;
        }

        public static EmployeeTimeChimp UpdateEmployee(EmployeeTimeChimp employee)
        {
            var client = new BearerTokenHttpClient();

            employee.id = GetEmployees().Find(e => e.userName.Equals(employee.userName)).id;
            employee.userName = null;
            

            String json = JsonTool.ConvertFrom(employee);
            String response = client.PutAsync("users", json).Result;

            EmployeeTimeChimp employeeResponse = JsonTool.ConvertTo<EmployeeTimeChimp>(response);
            return employeeResponse;
        }
    }
}
