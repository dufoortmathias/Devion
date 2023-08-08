namespace Api.Devion.Helpers.TimeChimp
{
    public class TimeChimpEmployeeHelper : TimeChimpHelper
    {
        public TimeChimpEmployeeHelper(BearerTokenHttpClient client) : base(client)
        {
        }

        public Boolean EmployeeExists(String employeeId)
        {
            return GetEmployees().Any(employee => employee.employeeNumber != null && employee.employeeNumber.Equals(employeeId));
        }

        public List<EmployeeTimeChimp> GetEmployees()
        {
            String response = TCClient.GetAsync("v1/users").Result;

            List<EmployeeTimeChimp> employees = JsonTool.ConvertTo<List<EmployeeTimeChimp>>(response);
            return employees;
        }

        public EmployeeTimeChimp CreateEmployee(EmployeeTimeChimp employee)
        {
            employee.email = employee.userName;
            employee.sendInvitation = true;

            String json = JsonTool.ConvertFrom(employee);
            String response = TCClient.PostAsync($"v1/users", json).Result;

            EmployeeTimeChimp employeeResponse = JsonTool.ConvertTo<EmployeeTimeChimp>(response);
            return employeeResponse;
        }

        public EmployeeTimeChimp UpdateEmployee(EmployeeTimeChimp employee)
        {
            employee.id = GetEmployees().Find(e => e.userName != null && e.userName.Equals(employee.userName)).id;            
            
            String json = JsonTool.ConvertFrom(employee);
            String response = TCClient.PutAsync("v1/users", json).Result;

            EmployeeTimeChimp employeeResponse = JsonTool.ConvertTo<EmployeeTimeChimp>(response);
            return employeeResponse;
        }

        public EmployeeTimeChimp GetEmployee(Int32 employeeId)
        {
            String response = TCClient.GetAsync($"v1/users/{employeeId}").Result;

            EmployeeTimeChimp employeeResponse = JsonTool.ConvertTo<EmployeeTimeChimp>(response);
            return employeeResponse;
        }
    }
}