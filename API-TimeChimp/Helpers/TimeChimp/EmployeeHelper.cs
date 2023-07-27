namespace Api.Devion.Helpers.TimeChimp
{
    public static class TimeChimpEmployeeHelper
    {
        public static EmployeeTimeChimp[] GetEmployees()
        {
            var client = new BearerTokenHttpClient();

            String response = client.GetAsync("users").Result;

            EmployeeTimeChimp[] employees = JsonTool.ConvertTo<EmployeeTimeChimp[]>(response);
            return employees;
        }

        public static EmployeeTimeChimp CreateEmployee(EmployeeTimeChimp employee)
        {
            var client = new BearerTokenHttpClient();

            String response = client.PostAsync("users", JsonTool.ConvertFrom(employee)).Result;

            EmployeeTimeChimp employeeResponse = JsonTool.ConvertTo<EmployeeTimeChimp>(response);
            return employeeResponse;
        }

        public static EmployeeTimeChimp UpdateEmployee(EmployeeTimeChimp employee)
        {
            var client = new BearerTokenHttpClient();

            String response = client.PutAsync("users", JsonTool.ConvertFrom(employee)).Result;

            EmployeeTimeChimp employeeResponse = JsonTool.ConvertTo<EmployeeTimeChimp>(response);
            return employeeResponse;
        }
    }
}
