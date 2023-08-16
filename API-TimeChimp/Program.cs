using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

int companyIndex = -1;
while (config[$"Companies:{++companyIndex}:Name"] != null)
{
    BearerTokenHttpClient TimeChimpClient = new(config["TimeChimpBaseURL"], config[$"Companies:{companyIndex}:TimeChimpToken"]);
    FirebirdClientETS ETSClient = new(config["ETSServer"], config[$"Companies:{companyIndex}:ETSUser"], config[$"Companies:{companyIndex}:ETSPassword"], config[$"Companies:{companyIndex}:ETSDatabase"]);

    String company = config[$"Companies:{companyIndex}:Name"];

    //get customers from timechimp
    app.MapGet($"/api/{company.ToLower()}/timechimp/customers", () => { try { return Results.Ok(new TimeChimpCustomerHelper(TimeChimpClient).GetCustomers()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetCustomers");

    //create customer in timechimp
    app.MapPost($"/api/{company.ToLower()}/timechimp/customer", (customerTimeChimp customer) => { try { return Results.Ok(new TimeChimpCustomerHelper(TimeChimpClient).CreateCustomer(customer)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}CreateCustomer");

    //get projects from timechimp
    app.MapGet($"/api/{company.ToLower()}/timechimp/projects", () => { try { return Results.Ok(new TimeChimpProjectHelper(TimeChimpClient).GetProjects()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetProjects");

    //create project in timechimp
    app.MapPost($"/api/{company.ToLower()}/timechimp/project", (ProjectTimeChimp project) => { try { return Results.Ok(new TimeChimpProjectHelper(TimeChimpClient).CreateProject(project)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}CreateProject");

    //update project in timechimp
    app.MapPut($"/api/{company.ToLower()}/timechimp/project", (ProjectTimeChimp project) => { try { return Results.Ok(new TimeChimpProjectHelper(TimeChimpClient).UpdateProject(project)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}UpdateProject");

    //get times from timechimp last week
    app.MapGet($"/api/{company.ToLower()}/timechimp/times", () => { try { return Results.Ok(new TimeChimpTimeHelper(TimeChimpClient, ETSClient).GetTimes(DateTime.Now.AddDays(-7))); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetTimesFromLastWeek");

    //update employee in timechimp
    app.MapPut($"/api/{company.ToLower()}/timechimp/employee", (EmployeeTimeChimp employee) => { try { return Results.Ok(new TimeChimpEmployeeHelper(TimeChimpClient).UpdateEmployee(employee)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}UpdateEmployee");

    //create employee in timechimp
    app.MapPost($"/api/{company.ToLower()}/timechimp/employee", (EmployeeTimeChimp employee) => { try { return Results.Ok(new TimeChimpEmployeeHelper(TimeChimpClient).CreateEmployee(employee)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}CreateEmployee");

    //get employees from timechimp
    app.MapGet($"/api/{company.ToLower()}/timechimp/employees", () => { try { return Results.Ok(new TimeChimpEmployeeHelper(TimeChimpClient).GetEmployees()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetEmployees");

    //get contacts from timechimp
    app.MapGet($"/api/{company.ToLower()}/timechimp/contacts", () => { try { return Results.Ok(new TimeChimpContactHelper(TimeChimpClient).GetContacts()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetContacts");

    //create contact in timechimp
    app.MapPost($"/api/{company.ToLower()}/timechimp/contact", (contactsTimeChimp contact) => { try { return Results.Ok(new TimeChimpContactHelper(TimeChimpClient).CreateContact(contact)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}PostContact");

    //update contact in timechimp
    app.MapPut($"/api/{company.ToLower()}/timechimp/contacten", (contactsTimeChimp contact) => { try { return Results.Ok(new TimeChimpContactHelper(TimeChimpClient).UpdateContact(contact)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}PutContact");

    //get mileages from timechimp and send the to ets
    app.MapGet($"/api/{company.ToLower()}/timechimp/mileage", () => { try { return Results.Ok(new TimeChimpMileageHelper(TimeChimpClient).GetMileages()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetMileages");

    //get customerids from ets
    app.MapGet($"/api/{company.ToLower()}/ets/customerids", (String dateString) => { try { return Results.Ok(new ETSCustomerHelper(ETSClient).GetCustomerIdsChangedAfter(DateTime.Parse(dateString))); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetCustomerIds");

    //sync customer from ets to timechimp
    app.MapPost($"/api/{company.ToLower()}/ets/synccustomer", (String customerId) =>
    {
        try
        {
            //get customer from ets
            CustomersETS ETSCustomer = new ETSCustomerHelper(ETSClient).GetCustomer(customerId);

            // Handle when customer doesn't exist in ETS
            if (ETSCustomer == null)
            {
                return Results.Problem($"ETS doesn't contain a customer with id = {customerId}");
            }

            //change to timechimp class
            customerTimeChimp TCCustomer = new(ETSCustomer);

            TimeChimpCustomerHelper customerHelper = new(TimeChimpClient);

            //check if customer exists in timechimp
            if (customerHelper.CustomerExists(customerId))
            {
                return Results.Ok(customerHelper.UpdateCustomer(TCCustomer));
            }
            else
            {
                return Results.Ok(customerHelper.CreateCustomer(TCCustomer));
            }
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}SyncCustomerTimechimp");

    //get contactids from ets
    app.MapGet($"/api/{company.ToLower()}/ets/contactids", (String dateString) => { try { return Results.Ok(new ETSContactHelper(ETSClient).GetContactIdsChangedAfter(DateTime.Parse(dateString))); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetContactIds");

    //sync contact from ets to timechimp
    app.MapPost($"/api/{company.ToLower()}/ets/synccontact", (Int32 contactId) =>
    {
        //get contact from ets
        contactsETS ETSContact = new ETSContactHelper(ETSClient).GetContact(contactId);

        // Handle when contact doesn't exist in ETS
        if (ETSContact == null)
        {
            return Results.Problem($"ETS doesn't contain a contact with id = {contactId}");
        }

        //change to timechimp class
        contactsTimeChimp TCContact = new(ETSContact);

        TimeChimpContactHelper contactHelper = new(TimeChimpClient);

        //check if contact exists in timechimp
        if (contactHelper.ContactExists(ETSContact))
        {
            return Results.Ok(contactHelper.UpdateContact(TCContact));
        }
        else
        {
            return Results.Ok(contactHelper.CreateContact(TCContact));
        }
    }).WithName($"{company}SyncContactTimechimp");

    //get projectids from ets
    app.MapGet($"/api/{company.ToLower()}/ets/projectids", (String dateString) => { try { return Results.Ok(new ETSProjectHelper(ETSClient).GetProjectIdsChangedAfter(DateTime.Parse(dateString))); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetProjectIds");

    //sync project from ets to timechimp
    app.MapPost($"/api/{company.ToLower()}/ets/syncproject", (String projectId) =>
    {
        try
        {
            ETSProjectHelper projectHelperETS = new(ETSClient);
            TimeChimpProjectHelper projectHelperTC = new(TimeChimpClient);

            // Get project from ETS
            ProjectETS ETSProject = projectHelperETS.GetProject(projectId);

            // Handle when project doesn't exist in ETS
            if (ETSProject == null)
            {
                return Results.Problem($"ETS doesn't contain a project with id = {projectId}");
            }
            else if (ETSProject.PR_KLNR == null)
            {
                return Results.Problem($"The ETS record for project with id = {projectId} doesn't has a customernumber");
            }

            // Change to TimeChimp class
            ProjectTimeChimp TCProject = new(ETSProject);

            // check if there is a client
            if (ETSProject.PR_KLNR != null)
            {
                TCProject.customerId = new TimeChimpCustomerHelper(TimeChimpClient).GetCustomers().Find(c => c.relationId != null && c.relationId.Equals(ETSProject.PR_KLNR)).id.Value;
            }
            else
            {
                TCProject.customerId = 0;
            }

            ProjectTimeChimp createdMainProject;

            // Check if project exists in TimeChimp
            if (projectHelperTC.ProjectExists(projectId))
            {
                createdMainProject = projectHelperTC.UpdateProject(TCProject);
            }
            else
            {
                createdMainProject = projectHelperTC.CreateProject(TCProject);
                createdMainProject = projectHelperTC.UpdateProject(TCProject);
            }

            // get subprojects from ETS
            List<SubprojectETS> ETSSubprojects = projectHelperETS.GetSubprojects(projectId);
            foreach (SubprojectETS ETSSubproject in ETSSubprojects)
            {
                // Change to TimeChimp class
                ProjectTimeChimp TCSubproject = new(ETSSubproject, TCProject);
                TCSubproject.mainProjectId = TCProject.id;

                if (projectHelperTC.ProjectExists(TCSubproject.code))
                {
                    projectHelperTC.UpdateProject(TCSubproject);
                }
                else
                {
                    projectHelperTC.CreateProject(TCSubproject);
                    projectHelperTC.UpdateProject(TCSubproject);
                }
            }

            return Results.Ok(projectHelperTC.GetProject(createdMainProject.id.Value));
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}SyncProjectTimechimp");

    //get employeeids from ets
    app.MapGet($"/api/{company.ToLower()}/ets/employeeids", (String dateString, String teamName) => { try { return Results.Ok(new ETSEmployeeHelper(ETSClient).GetEmployeeIdsChangedAfter(DateTime.Parse(dateString), teamName)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetEmployeeIds");

    //get timesids from timechimp
    app.MapGet($"/api/{company.ToLower()}/ets/timeids", (String dateString) => { try { return Results.Ok(new TimeChimpTimeHelper(TimeChimpClient, ETSClient).GetTimes(DateTime.Parse(dateString))); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetTimeIds");

    //sync time from timechimp to ets
    app.MapPost($"/api/{company.ToLower()}/ets/synctime", (Int32 timeId) =>
    {
        try
        {
            ETSTimeHelper timeHelperETS = new(ETSClient, TimeChimpClient);
            TimeChimpTimeHelper timeHelperTC = new(TimeChimpClient, ETSClient);

            // Get time from TimeChimp
            TimeTimeChimp TCTime = timeHelperTC.GetTime(timeId);

            // Handle when time doesn't exist in TimeChimp
            if (TCTime == null)
            {
                return Results.Problem($"TimeChimp doesn't contain a time with id = {timeId}");
            }

            // add time to ETS
            timeHelperETS.AddTime(TCTime);

            //change status to invoiced (3)
            TimeTimeChimp time = timeHelperTC.InvoiceTime(timeId);

            return Results.Ok(time);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}SyncTimeETS");

    //sync employee from ets to timechimp
    app.MapPost($"/api/{company.ToLower()}/ets/syncemployee", (String employeeId) =>
    {
        try
        {
            //get employee from ets
            EmployeeETS ETSEmployee = new ETSEmployeeHelper(ETSClient).GetEmployee(employeeId);

            // Handle when contact doesn't exist in ETS
            if (ETSEmployee == null)
            {
                return Results.Problem($"ETS doesn't contain an employee with id = {employeeId}");
            }

            //change to timechimp class
            EmployeeTimeChimp TCEmployee = new(ETSEmployee);

            TimeChimpEmployeeHelper employeeHelper = new(TimeChimpClient);

            //check if employee exists in timechimp
            if (employeeHelper.EmployeeExists(employeeId))
            {
                return Results.Ok(employeeHelper.UpdateEmployee(TCEmployee));
            }
            else
            {
                //check if employee has an emailaddress
                if (TCEmployee.userName == null)
                {
                    return Results.Problem($"Can't create the employee {TCEmployee.displayName} without an emailaddress");
                }

                EmployeeTimeChimp employee = employeeHelper.CreateEmployee(TCEmployee);
                TCEmployee.id = employee.id;
                employee = employeeHelper.UpdateEmployee(TCEmployee);

                //adds employee to all existing projects in TimeChimp
                new TimeChimpProjectUserHelper(TimeChimpClient).AddAllProjectUserForEmployee(employee.id.Value);

                return Results.Ok(employee);
            }
        }
        catch (Exception exception)
        {
            return Results.Problem(exception.Message);
        }

    }).WithName($"{company}SyncEmployeeTimechimp");

    //get mileageids from timechimp that were changed after specific time
    app.MapGet($"/api/{company.ToLower()}/ets/mileageids", (String dateString) =>
    {
        try
        {
            return Results.Ok(new TimeChimpMileageHelper(TimeChimpClient).GetApprovedMileageIdsByDate(DateTime.Parse(dateString)));
        }
        catch (Exception exception)
        {
            return Results.Problem(exception.Message);
        }
    }).WithName($"{company}GetMileageIds");

    //sync mileages from timechimp to ets
    app.MapPost($"/api/{company.ToLower()}/ets/syncmileage", (Int32 mileageId) =>
    {
        try
        {
            MileageTimeChimp mileage = new TimeChimpMileageHelper(TimeChimpClient).GetMileage(mileageId);

            if (mileage.status == 3)
            {
                throw new Exception($"Mileage with id ({mileageId}) already invoiced");
            }

            mileage.projectId = new TimeChimpProjectHelper(TimeChimpClient).GetProjectId(mileage.projectId);
            mileage.userId = int.Parse(new TimeChimpEmployeeHelper(TimeChimpClient).GetEmployee(mileage.userId).employeeNumber);

            MileageETS mileageETS = new MileageETS(mileage);
            var response = new ETSMileageHelper(ETSClient).UpdateMileage(mileageETS);

            //change status
            var responseStatus = new TimeChimpMileageHelper(TimeChimpClient).changeStatus(mileageId);

            return Results.Ok(response);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}GetMileagesFromETS");

    //get mileages from timechimp
    app.MapGet($"/api/{company.ToLower()}/timechimp/mileages", () => { try { return Results.Ok(new TimeChimpMileageHelper(TimeChimpClient).GetMileages()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetMileagesFromTimechimp");

    //get uurcodes from timechimp
    app.MapGet($"/api/{company.ToLower()}/timechimp/uurcodes", () => { try { return Results.Ok(new TimeChimpUurcodeHelper(TimeChimpClient, ETSClient).GetUurcodes()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetUurcodes");

    //get uurcodes from ets
    app.MapGet($"/api/{company.ToLower()}/ets/uurcodeids", (string dateString) => { try { return Results.Ok(new ETSUurcodeHelper(ETSClient).GetUurcodes(DateTime.Parse(dateString))); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetUurcodesFromETS");

    //sync uurcodes from ets to timechimp
    app.MapPost($"/api/{company.ToLower()}/ets/syncuurcode", (string uurcodeId) =>
    {
        try
        {
            //get uurcode from ets
            uurcodesETS ETSUurcode = new ETSUurcodeHelper(ETSClient).GetUurcode(uurcodeId);

            // Handle when uurcode doesn't exist in ETS
            if (ETSUurcode == null)
            {
                return Results.Problem($"ETS doesn't contain an uurcode with id = {uurcodeId}");
            }

            //change to timechimp class
            uurcodesTimeChimp TCUurcode = new(ETSUurcode);

            TimeChimpUurcodeHelper uurcodeHelper = new(TimeChimpClient, ETSClient);

            //check if uurcode exists in timechimp
            if (uurcodeHelper.uurcodeExists(uurcodeId))
            {
                return Results.Ok(uurcodeHelper.UpdateUurcode(TCUurcode));
            }
            else
            {
                return Results.Ok(uurcodeHelper.CreateUurcode(TCUurcode));
            }
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}UpdateUurcodes");

    //get subprojects from ets
    app.MapGet($"/api/{company.ToLower()}/ets/subprojects", (string mainprojectid) => { try { return Results.Ok(new ETSProjectHelper(ETSClient).GetSubprojects(mainprojectid)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetSubprojects");

    //get projectusers from timechimp
    app.MapGet($"/api/{company.ToLower()}/timechimp/projectusers", () => { try { return Results.Ok(new TimeChimpProjectUserHelper(TimeChimpClient).GetProjectUsers()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetProjectUsers");
}
app.Run();