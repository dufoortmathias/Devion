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
            return GetEmployees().Any(employee => employee.EmployeeNumber != null && employee.EmployeeNumber.Equals(employeeId));
        }

        //get all employees
        public List<EmployeeTimeChimp> GetEmployees()
        {
            //get data from timechimp
            string response = TCClient.GetAsync("users?&$filter=active eq true&$orderby=employeeNumber");

            //convert data to employeeTimeChimp object
            Console.WriteLine(response);

            ResponseTCEmployee result = JsonTool.ConvertTo<ResponseTCEmployee>(response) ?? throw new Exception("Error getting employees from TimeChimp");
            List<EmployeeTimeChimp> employees = result.Result.ToList();
            return employees;
        }

        //create employee
        public EmployeeTimeChimp CreateEmployee(EmployeeTimeChimp employee)
        {
            //set properties
            employee.UserName = employee.UserName;
            employee.SendInvitation = true;

            //create json
            string json = JsonTool.ConvertFrom(employee) ?? throw new Exception("Error converting employee to json");

            //send data to timechimp
            string response = TCClient.PostAsync($"users", json);

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
            EmployeeTimeChimp employeeFound = GetEmployees().Find(e => e.UserName != null && e.UserName.Equals(employee.UserName)) ?? throw new Exception($"Timechimp has no employee with UserName = {employee.UserName} to update");
            employee.Id = employeeFound.Id;

            //create json
            string json = JsonTool.ConvertFrom(employee) ?? throw new Exception("Error converting employee to json");

            //send data to timechimp
            string response = TCClient.PutAsync($"users/{employee.Id}", json);

            //convert response to employeeTimeChimp object
            EmployeeTimeChimp employeeResponse = JsonTool.ConvertTo<EmployeeTimeChimp>(response);
            return employeeResponse;
        }

        //get employee by id
        public EmployeeTimeChimp GetEmployee(int employeeId)
        {
            //get data form timechimp
            string response = TCClient.GetAsync($"users/{employeeId}");

            //convert data to employeeTimeChimp object
            EmployeeTimeChimp employeeResponse = JsonTool.ConvertTo<EmployeeTimeChimp>(response);
            return employeeResponse;
        }
    }
}