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

            employee.email = employee.userName;
            employee.sendInvitation = true;

            String json = JsonTool.ConvertFrom(employee);
            String response = client.PostAsync($"users", json).Result;

            EmployeeTimeChimp employeeResponse = JsonTool.ConvertTo<EmployeeTimeChimp>(response);
            return employeeResponse;
        }

        public static EmployeeTimeChimp UpdateEmployee(EmployeeTimeChimp employee)
        {
            var client = new BearerTokenHttpClient();

            employee.id = GetEmployees().Find(e => e.userName != null && e.userName.Equals(employee.userName)).id;            
            
            String json = JsonTool.ConvertFrom(employee);
            String response = client.PutAsync("users", json).Result;

            EmployeeTimeChimp employeeResponse = JsonTool.ConvertTo<EmployeeTimeChimp>(response);
            return employeeResponse;
        }

        public static EmployeeTimeChimp GetEmployee(string employeeId)
        {
            return GetEmployees().Find(e => e.employeeNumber.Equals(employeeId));
        }
    }
}