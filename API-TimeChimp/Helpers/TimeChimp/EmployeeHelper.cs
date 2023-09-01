namespace Api.Devion.Helpers.TimeChimp
{
    public class TimeChimpEmployeeHelper : TimeChimpHelper
    {
        public TimeChimpEmployeeHelper(WebClient client) : base(client)
        {
        }

        //chek if employee exists
        public bool EmployeeExists(string employeeId)
        {
            return GetEmployees().Any(employee => employee.employeeNumber != null && employee.employeeNumber.Equals(employeeId));
        }

        //get all employees
        public List<EmployeeTimeChimp> GetEmployees()
        {
            //get data from timechimp
            string response = TCClient.GetAsync("v1/users");

            //convert data to employeeTimeChimp object
            List<EmployeeTimeChimp> employees = JsonTool.ConvertTo<List<EmployeeTimeChimp>>(response);
            return employees;
        }

        //create employee
        public EmployeeTimeChimp CreateEmployee(EmployeeTimeChimp employee)
        {
            //set properties
            employee.email = employee.userName;
            employee.sendInvitation = true;

            //create json
            string json = JsonTool.ConvertFrom(employee);

            //check if json is not empty
            if (json == null)
            {
                throw new Exception("Error converting employee to json");
            }

            //send data to timechimp
            string response = TCClient.PostAsync($"v1/users", json);

            //throw an error when post request fails with status WaitingForActivation
            //this status means an employee already exists with that email and is waiting for approval
            //TODO rewrite
            //if (response.Status.Equals(TaskStatus.WaitingForActivation))
            //{
            //    throw new Exception($"In TimeChimp an employee with email \"{employee.email}\" already exists that is not active");
            //}

            //convert response to employeeTimeChimp object
            EmployeeTimeChimp employeeResponse = JsonTool.ConvertTo<EmployeeTimeChimp>(response);
            return employeeResponse;
        }

        //update employee
        public EmployeeTimeChimp UpdateEmployee(EmployeeTimeChimp employee)
        {
            //get employeeid
            EmployeeTimeChimp employeeFound = GetEmployees().Find(e => e.userName != null && e.userName.Equals(employee.userName)) ?? throw new Exception($"Timechimp has no employee with userName = {employee.userName} to update");
            employee.id = employeeFound.id;

            //create json
            string json = JsonTool.ConvertFrom(employee) ?? throw new Exception("Error converting employee to json");

            //send data to timechimp
            string response = TCClient.PutAsync("v1/users", json);

            //convert response to employeeTimeChimp object
            EmployeeTimeChimp employeeResponse = JsonTool.ConvertTo<EmployeeTimeChimp>(response);
            return employeeResponse;
        }

        //get employee by id
        public EmployeeTimeChimp GetEmployee(int employeeId)
        {
            //get data form timechimp
            string response = TCClient.GetAsync($"v1/users/{employeeId}");

            //convert data to employeeTimeChimp object
            EmployeeTimeChimp employeeResponse = JsonTool.ConvertTo<EmployeeTimeChimp>(response);
            return employeeResponse;
        }
    }
}