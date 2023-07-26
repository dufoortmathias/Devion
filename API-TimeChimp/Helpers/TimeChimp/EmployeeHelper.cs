namespace Api.Devion.Helpers.TimeChimp
{
    public static class TimeChimpEmployeeHelper
    {
        public static EmployeeTimeChimp[] GetEmployees()
        {
            var client = new BearerTokenHttpClient();

            String response = client.GetAsync("users").Result;

            EmployeeTimeChimp[] employees = JsonConvert.DeserializeObject<EmployeeTimeChimp[]>(response);
            return employees;
        }

        public static EmployeeTimeChimp CreateEmployee(EmployeeTimeChimp employee)
        {
            var client = new BearerTokenHttpClient();

            String response = client.PostAsync("users", JsonConvert.SerializeObject(employee)).Result;

            EmployeeTimeChimp employeeResponse = JsonConvert.DeserializeObject<EmployeeTimeChimp>(response);
            return employeeResponse;
        }

        public static EmployeeTimeChimp UpdateEmployee(EmployeeTimeChimp employee)
        {
            var client = new BearerTokenHttpClient();

            String response = client.PutAsync("users", JsonConvert.SerializeObject(employee)).Result;

            EmployeeTimeChimp employeeResponse = JsonConvert.DeserializeObject<EmployeeTimeChimp>(response);
            return employeeResponse;
        }
    }
}
