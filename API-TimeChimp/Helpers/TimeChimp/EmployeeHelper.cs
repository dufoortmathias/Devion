namespace Api.Devion.Helpers.TimeChimp
{
    public static class TimeChimpEmployeeHelper
    {
        public static Boolean EmployeeExists(String employeeId)
        {
            return GetEmployees().Any(employee => employee.employeeNumber != null && employee.employeeNumber.Equals(employeeId));
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
            var json = JsonTool.ConvertFrom(employee);
            String response = client.PostAsync("users", json).Result;

            employee.email = employee.userName;
            employee.sendInvitation = true;

            String response = client.PostAsync("users", JsonTool.ConvertFrom(employee)).Result;

            var client = new BearerTokenHttpClient();

            String response = client.PutAsync($"users", JsonTool.ConvertFrom(employee)).Result;
            Console.WriteLine(response);
            EmployeeTimeChimp employeeResponse = JsonTool.ConvertTo<EmployeeTimeChimp>(response);
            return employeeResponse;
        }

        public static EmployeeTimeChimp UpdateEmployee(String employeeId)
        {
            var client = new BearerTokenHttpClient();

            employee.id = GetEmployees().Find(e => e.employeeNumber.Equals(employeeId)).id;
            employee.userName = null;
            

            String json = JsonTool.ConvertFrom(employee);
            String response = client.PutAsync("users", json).Result;

            EmployeeTimeChimp employeeResponse = JsonTool.ConvertTo<EmployeeTimeChimp>(response);
            return employeeResponse;
        }
    }
}