namespace Api.Devion.Helpers.TimeChimp
{
    public static class TimeChimpEmployeeHelper
    {
        public static Boolean EmployeeExists(EmployeeETS employee)
        {
            return GetEmployees().Any(employee2 => employee2.displayName != null && employee2.displayName.Equals(employee.PN_NAM));
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
            EmployeeTimeChimp employee2 = GetEmployees().ToList().Find(e => e.displayName == employee.displayName);

            if (employee2 != null)
            {
                if (employee.id == null)
                {
                    employee.id = GetEmployees().ToList().Find(e => e.displayName == employee.displayName).id;
                }
            }

            var client = new BearerTokenHttpClient();
            String response = client.PutAsync($"users", JsonTool.ConvertFromWithNullValues(employee)).Result;
            return employee;
        }

        public static EmployeeTimeChimp GetEmployee(Int32 employeeId)
        {
            var client = new BearerTokenHttpClient();

            String response = client.GetAsync($"users/{employeeId}").Result;

            EmployeeTimeChimp employeeResponse = JsonTool.ConvertTo<EmployeeTimeChimp>(response);
            return employeeResponse;
        }
    }
}