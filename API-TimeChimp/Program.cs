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
    BearerTokenHttpClient TCClient = new(config["TimeChimpBaseURL"], config[$"Companies:{companyIndex}:TimeChimpToken"]);
    FirebirdClientETS ETSClient = new(config["ETSServer"], config[$"Companies:{companyIndex}:ETSUser"], config[$"Companies:{companyIndex}:ETSPassword"], config[$"Companies:{companyIndex}:ETSDatabase"]);

    String company = config[$"Companies:{companyIndex}:Name"];

    //get customers from timechimp
    app.MapGet($"/api/{company.ToLower()}/timechimp/customers", () => { try { return Results.Ok(new TimeChimpCustomerHelper(TCClient).GetCustomers()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetCustomers");

    //create customer in timechimp
    app.MapPost($"/api/{company.ToLower()}/timechimp/customer", (CustomerTimeChimp customer) => { try { return Results.Ok(new TimeChimpCustomerHelper(TCClient).CreateCustomer(customer)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}CreateCustomer");

    //get projects from timechimp
    app.MapGet($"/api/{company.ToLower()}/timechimp/projects", () => { try { return Results.Ok(new TimeChimpProjectHelper(TCClient).GetProjects()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetProjects");

    //create project in timechimp
    app.MapPost($"/api/{company.ToLower()}/timechimp/project", (ProjectTimeChimp project) => { try { return Results.Ok(new TimeChimpProjectHelper(TCClient).CreateProject(project)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}CreateProject");

    //update project in timechimp
    app.MapPut($"/api/{company.ToLower()}/timechimp/project", (ProjectTimeChimp project) => { try { return Results.Ok(new TimeChimpProjectHelper(TCClient).UpdateProject(project)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}UpdateProject");

    //get times from timechimp last week
    app.MapGet($"/api/{company.ToLower()}/timechimp/times", () => { try { return Results.Ok(new TimeChimpTimeHelper(TCClient, ETSClient).GetTimes(DateTime.Now.AddDays(-7))); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetTimesFromLastWeek");

    //update employee in timechimp
    app.MapPut($"/api/{company.ToLower()}/timechimp/employee", (EmployeeTimeChimp employee) => { try { return Results.Ok(new TimeChimpEmployeeHelper(TCClient).UpdateEmployee(employee)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}UpdateEmployee");

    //create employee in timechimp
    app.MapPost($"/api/{company.ToLower()}/timechimp/employee", (EmployeeTimeChimp employee) => { try { return Results.Ok(new TimeChimpEmployeeHelper(TCClient).CreateEmployee(employee)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}CreateEmployee");

    //get employees from timechimp
    app.MapGet($"/api/{company.ToLower()}/timechimp/employees", () => { try { return Results.Ok(new TimeChimpEmployeeHelper(TCClient).GetEmployees()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetEmployees");

    //get contacts from timechimp
    app.MapGet($"/api/{company.ToLower()}/timechimp/contacts", () => { try { return Results.Ok(new TimeChimpContactHelper(TCClient).GetContacts()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetContacts");

    //create contact in timechimp
    app.MapPost($"/api/{company.ToLower()}/timechimp/contact", (ContactTimeChimp contact) => { try { return Results.Ok(new TimeChimpContactHelper(TCClient).CreateContact(contact)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}PostContact");

    //update contact in timechimp
    app.MapPut($"/api/{company.ToLower()}/timechimp/contacten", (ContactTimeChimp contact) => { try { return Results.Ok(new TimeChimpContactHelper(TCClient).UpdateContact(contact)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}PutContact");

    //get mileages from timechimp and send the to ets
    app.MapGet($"/api/{company.ToLower()}/timechimp/mileage", () => { try { return Results.Ok(new TimeChimpMileageHelper(TCClient).GetMileages()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetMileages");

    //get customerids from ets
    app.MapGet($"/api/{company.ToLower()}/ets/customerids", (String dateString) => { try { return Results.Ok(new ETSCustomerHelper(ETSClient).GetCustomerIdsChangedAfter(DateTime.Parse(dateString))); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetCustomerIds");

    //sync customer from ets to timechimp
    app.MapPost($"/api/{company.ToLower()}/ets/synccustomer", (String customerId) =>
    {
        try
        {
            //get customer from ets
            CustomerETS ETSCustomer = new ETSCustomerHelper(ETSClient).GetCustomer(customerId);

            // Handle when customer doesn't exist in ETS
            if (ETSCustomer == null)
            {
                return Results.Problem($"ETS doesn't contain a customer with id = {customerId}");
            }

            //change to timechimp class
            CustomerTimeChimp TCCustomer = new(ETSCustomer);

            TimeChimpCustomerHelper customerHelper = new(TCClient);

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
        ContactETS ETSContact = new ETSContactHelper(ETSClient).GetContact(contactId);

        // Handle when contact doesn't exist in ETS
        if (ETSContact == null)
        {
            return Results.Problem($"ETS doesn't contain a contact with id = {contactId}");
        }

        //change to timechimp class
        ContactTimeChimp TCContact = new(ETSContact);

        TimeChimpContactHelper contactHelper = new(TCClient);

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
            TimeChimpProjectHelper projectHelperTC = new(TCClient);

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
            ProjectTimeChimp TCProject = new(ETSProject)
            {
                // Find customer id TimeChimp
                customerId = new TimeChimpCustomerHelper(TCClient).GetCustomers().Find(c => c.relationId != null && c.relationId.Equals(ETSProject.PR_KLNR)).id.Value
            };

            ProjectTimeChimp mainProject = projectHelperTC.FindProject(projectId);

            // Check if project exists in TimeChimp
            if (mainProject != null)
            {
                TCProject.id = mainProject.id;
                mainProject = projectHelperTC.UpdateProject(TCProject);
            }
            else
            {
                mainProject = projectHelperTC.CreateProject(TCProject);
                TCProject.id = mainProject.id;
                mainProject = projectHelperTC.UpdateProject(TCProject);
            }

            // get subprojects from ETS
            List<SubprojectETS> ETSSubprojects = projectHelperETS.GetSubprojects(projectId);
            foreach (SubprojectETS ETSSubproject in ETSSubprojects)
            {
                // Change to TimeChimp class
                ProjectTimeChimp TCSubproject = new(ETSSubproject, TCProject)
                {
                    mainProjectId = TCProject.id
                };

                ProjectTimeChimp subProject = projectHelperTC.FindProject(TCSubproject.code);

                if (subProject != null)
                {
                    TCSubproject.id = subProject.id;
                    subProject = projectHelperTC.UpdateProject(TCSubproject);
                }
                else
                {
                    subProject = projectHelperTC.CreateProject(TCSubproject);
                    TCSubproject.id = subProject.id;
                    subProject = projectHelperTC.UpdateProject(TCSubproject);
                }
            }

            return Results.Ok(projectHelperTC.GetProject(TCProject.id.Value));
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}SyncProjectTimechimp");

    //get employeeids from ets
    app.MapGet($"/api/{company.ToLower()}/ets/employeeids", (String dateString, String teamName) => { try { return Results.Ok(new ETSEmployeeHelper(ETSClient).GetEmployeeIdsChangedAfter(DateTime.Parse(dateString), teamName)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetEmployeeIds");

    //get timesids from timechimp
    app.MapGet($"/api/{company.ToLower()}/ets/timeids", (String dateString) => { try { return Results.Ok(new TimeChimpTimeHelper(TCClient, ETSClient).GetTimes(DateTime.Parse(dateString))); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetTimeIds");

    //sync time from timechimp to ets
    app.MapPost($"/api/{company.ToLower()}/ets/synctime", (Int32 timeId) =>
    {
        try
        {
            ETSTimeHelper timeHelperETS = new(ETSClient, TCClient);
            TimeChimpTimeHelper timeHelperTC = new(TCClient, ETSClient);

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

            TimeChimpEmployeeHelper employeeHelper = new(TCClient);

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
                new TimeChimpProjectUserHelper(TCClient).AddAllProjectUserForEmployee(employee.id.Value);

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
            return Results.Ok(new TimeChimpMileageHelper(TCClient).GetApprovedMileageIdsByDate(DateTime.Parse(dateString)));
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
            MileageTimeChimp mileage = new TimeChimpMileageHelper(TCClient).GetMileage(mileageId);

            if (mileage.status == 3)
            {
                throw new Exception($"Mileage with id ({mileageId}) already invoiced");
            }

            mileage.projectId = new TimeChimpProjectHelper(TCClient).GetProjectId(mileage.projectId);
            mileage.userId = int.Parse(new TimeChimpEmployeeHelper(TCClient).GetEmployee(mileage.userId).employeeNumber);

            MileageETS mileageETS = new MileageETS(mileage);
            var response = new ETSMileageHelper(ETSClient).UpdateMileage(mileageETS);

            //change status
            var responseStatus = new TimeChimpMileageHelper(TCClient).changeStatus(mileageId);

            return Results.Ok(response);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}GetMileagesFromETS");

    //get mileages from timechimp
    app.MapGet($"/api/{company.ToLower()}/timechimp/mileages", () => { try { return Results.Ok(new TimeChimpMileageHelper(TCClient).GetMileages()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetMileagesFromTimechimp");

    //get uurcodes from timechimp
    app.MapGet($"/api/{company.ToLower()}/timechimp/uurcodes", () => { try { return Results.Ok(new TimeChimpUurcodeHelper(TCClient, ETSClient).GetUurcodes()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetUurcodes");

    //get uurcodes from ets
    app.MapGet($"/api/{company.ToLower()}/ets/uurcodeids", (string dateString) => { try { return Results.Ok(new ETSUurcodeHelper(ETSClient).GetUurcodes(DateTime.Parse(dateString))); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetUurcodesFromETS");

    //sync uurcodes from ets to timechimp
    app.MapPost($"/api/{company.ToLower()}/ets/syncuurcode", (string uurcodeId) =>
    {
        try
        {
            //get uurcode from ets
            UurcodeETS ETSUurcode = new ETSUurcodeHelper(ETSClient).GetUurcode(uurcodeId);

            // Handle when uurcode doesn't exist in ETS
            if (ETSUurcode == null)
            {
                return Results.Problem($"ETS doesn't contain an uurcode with id = {uurcodeId}");
            }

            //change to timechimp class
            UurcodeTimeChimp TCUurcode = new(ETSUurcode);

            TimeChimpUurcodeHelper uurcodeHelper = new(TCClient, ETSClient);

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
    app.MapGet($"/api/{company.ToLower()}/timechimp/projectusers", () => { try { return Results.Ok(new TimeChimpProjectUserHelper(TCClient).GetProjectUsers()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetProjectUsers");
}
app.Run();