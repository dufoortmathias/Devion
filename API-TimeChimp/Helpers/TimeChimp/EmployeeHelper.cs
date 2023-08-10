namespace Api.Devion.Helpers.TimeChimp
{
    public class TimeChimpEmployeeHelper : TimeChimpHelper
    {
        public TimeChimpEmployeeHelper(BearerTokenHttpClient client) : base(client)
        {
        }

        //chek if employee exists
        public Boolean EmployeeExists(String employeeId)
        {
            return GetEmployees().Any(employee => employee.employeeNumber != null && employee.employeeNumber.Equals(employeeId));
        }

        //get all employees
        public List<EmployeeTimeChimp> GetEmployees()
        {
            //get data from timechimp
            var response = TCClient.GetAsync("v1/users");

            //check if response is succesfull
            if (!response.IsCompletedSuccessfully)
            {
                throw new Exception("Error getting all employees from timechimp with endpoint: v1/users");
            }

            //convert data to employeeTimeChimp object
            List<EmployeeTimeChimp> employees = JsonTool.ConvertTo<List<EmployeeTimeChimp>>(response.Result);
            return employees;
        }

        //create employee
        public EmployeeTimeChimp CreateEmployee(EmployeeTimeChimp employee)
        {
            //set properties
            employee.email = employee.userName;
            employee.sendInvitation = true;

            //create json
            String json = JsonTool.ConvertFrom(employee);

            //check if json is not empty
            if (json == null)
            {
                throw new Exception("Error converting employee to json");
            }

            //send data to timechimp
            String response = TCClient.PostAsync($"v1/users", json).Result;

            //throw an error when post request fails with status WaitingForActivation
            //this status means an employee already exists with that email and is waiting for approval
            if (response.Status.Equals(TaskStatus.WaitingForActivation))
            {
                throw new Exception($"In TimeChimp an employee with email \"{employee.email}\" already exists that is not active");
            }

            //convert response to employeeTimeChimp object
            EmployeeTimeChimp employeeResponse = JsonTool.ConvertTo<EmployeeTimeChimp>(response);
            return employeeResponse;
        }

        //update employee
        public EmployeeTimeChimp UpdateEmployee(EmployeeTimeChimp employee)
        {
            //get employeeid
            employee.id = GetEmployees().Find(e => e.userName != null && e.userName.Equals(employee.userName)).id;

            //create json
            String json = JsonTool.ConvertFrom(employee);

            //check if json is not empty
            if (json == null)
            {
                throw new Exception("Error converting employee to json");
            }

            //send data to timechimp
            var response = TCClient.PutAsync("v1/users", json);

            //check if response is succesfull
            if (!response.IsCompletedSuccessfully)
            {
                throw new Exception("Error updating employee in timechimp with endpoint: v1/users");
            }

            //convert response to employeeTimeChimp object
            EmployeeTimeChimp employeeResponse = JsonTool.ConvertTo<EmployeeTimeChimp>(response.Result);
            return employeeResponse;
        }

        //get employee by id
        public EmployeeTimeChimp GetEmployee(Int32 employeeId)
        {
            //get data form timechimp
            var response = TCClient.GetAsync($"v1/users/{employeeId}");

            //check if response is succesfull
            if (!response.IsCompletedSuccessfully)
            {
                throw new Exception($"Error getting employee from timechimp with endpoint: v1/users/{employeeId}");
            }

            //convert data to employeeTimeChimp object
            EmployeeTimeChimp employeeResponse = JsonTool.ConvertTo<EmployeeTimeChimp>(response.Result);
            return employeeResponse;
        }
    }
}