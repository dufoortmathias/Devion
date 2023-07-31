namespace Api.Devion.Helpers.TimeChimp
{
    public static class TimeChimpEmployeeHelper
    {
        public static Boolean EmployeeExists(String employeeId)
        {
            return GetEmployees().Any(employee => employee.employeeNumber != null && employee.employeeNumber.Equals(employeeId));
        }
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
            var json = JsonTool.ConvertFrom(employee);
            String response = client.PostAsync("users", json).Result;

            EmployeeTimeChimp employeeResponse = JsonTool.ConvertTo<EmployeeTimeChimp>(response);
            return employeeResponse;
        }

        public static EmployeeTimeChimp UpdateEmployee(EmployeeTimeChimp employee)
        {
            EmployeeTimeChimp employee2 = GetEmployees().ToList().Find(e => e.employeeNumber == employee.employeeNumber);
            Console.WriteLine(employee2.id);
            if (employee.id == null)
            {
                employee.id = GetEmployees().ToList().Find(e => e.employeeNumber == employee.employeeNumber).id;
            }

            var client = new BearerTokenHttpClient();

            String response = client.PutAsync($"users", JsonTool.ConvertFrom(employee)).Result;
            Console.WriteLine(response);
            EmployeeTimeChimp employeeResponse = JsonTool.ConvertTo<EmployeeTimeChimp>(response);
            return employeeResponse;
        }
    }
}