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

Console.WriteLine(config["TimeChimp:BaseURL"]);

// TimeChimp DEVION
// BearerTokenHttpClient TimeChimpClient = new(config["TimeChimp:BaseURL"], config["TimeChimp:BearerTokenDevion"]);

// TimeChimp METABIL
BearerTokenHttpClient TimeChimpClient = new(config["TimeChimp:BaseURL"], config["TimeChimp:BearerTokenMetabil"]);

// ETS DEVION
// FirebirdClientETS ETSClient = new(config["ETS:Server"], config["ETS:UserDevion"], config["ETS:PasswordDevion"], config["ETS:DatabaseDevion"]);

// ETS METABIL
FirebirdClientETS ETSClient = new(config["ETS:Server"], config["ETS:UserMetabil"], config["ETS:PasswordMetabil"], config["ETS:DatabaseMetabil"]);

//get customers from timechimp
app.MapGet("/api/timechimp/customers", () => new TimeChimpCustomerHelper(TimeChimpClient).GetCustomers()).WithName("GetCustomers");

//create customer in timechimp
app.MapPost("/api/timechimp/customer", (customerTimeChimp customer) => new TimeChimpCustomerHelper(TimeChimpClient).CreateCustomer(customer)).WithName("CreateCustomer");

//get projects from timechimp
app.MapGet("/api/timechimp/projects", () => new TimeChimpProjectHelper(TimeChimpClient).GetProjects()).WithName("GetProjects");

//create project in timechimp
app.MapPost("/api/timechimp/project", (ProjectTimeChimp project) => new TimeChimpProjectHelper(TimeChimpClient).CreateProject(project)).WithName("CreateProject");

//update project in timechimp
app.MapPut("/api/timechimp/project", (ProjectTimeChimp project) => new TimeChimpProjectHelper(TimeChimpClient).UpdateProject(project)).WithName("UpdateProject");

//get times from timechimp last week
app.MapGet("api/timechimp/times", () => new TimeChimpTimeHelper(TimeChimpClient, ETSClient).GetTimesLastWeek()).WithName("GetTimesFromLastWeek");

//update employee in timechimp
app.MapPut("/api/timechimp/employee", (EmployeeTimeChimp employee) => new TimeChimpEmployeeHelper(TimeChimpClient).UpdateEmployee(employee)).WithName("UpdateEmployee");

//create employee in timechimp
app.MapPost("/api/timechimp/employee", (EmployeeTimeChimp employee) => new TimeChimpEmployeeHelper(TimeChimpClient).CreateEmployee(employee)).WithName("CreateEmployee");

//get employees from timechimp
app.MapGet("/api/timechimp/employees", () => new TimeChimpEmployeeHelper(TimeChimpClient).GetEmployees()).WithName("GetEmployees");

//get contacts from timechimp
app.MapGet("/api/timechimp/contacts", () => new TimeChimpContactHelper(TimeChimpClient).GetContacts()).WithName("GetContacts");

//create contact in timechimp
app.MapPost("/api/timechimp/contact", (contactsTimeChimp contact) => new TimeChimpContactHelper(TimeChimpClient).CreateContact(contact)).WithName("PostContact");

//update contact in timechimp
app.MapPut("/api/timechimp/contacten", (contactsTimeChimp contact) => new TimeChimpContactHelper(TimeChimpClient).UpdateContact(contact)).WithName("PutContact");

//get mileages from timechimp and send the to ets
app.MapGet("/api/timechimp/mileage", () => new TimeChimpMileageHelper(TimeChimpClient).GetMileages()).WithName("GetMileages");

//get customerids from ets
app.MapGet("/api/ets/customerids", (String dateString) => new ETSCustomerHelper(ETSClient).GetCustomerIdsChangedAfter(DateTime.Parse(dateString))).WithName("GetCustomerIds");

//sync customer from ets to timechimp
app.MapPost("/api/ets/synccustomer", (String customerId) =>
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
}).WithName("SyncCustomerTimechimp");

//get contactids from ets
app.MapGet("/api/ets/contactids", (String dateString) => new ETSContactHelper(ETSClient).GetContactIdsChangedAfter(DateTime.Parse(dateString))).WithName("GetContactIds");

//sync contact from ets to timechimp
app.MapPost("/api/ets/synccontact", (Int32 contactId) =>
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
}).WithName("SyncContactTimechimp");

//get projectids from ets
app.MapGet("/api/ets/projectids", (String dateString) => new ETSProjectHelper(ETSClient).GetProjectIdsChangedAfter(DateTime.Parse(dateString))).WithName("GetProjectIds");

//sync project from ets to timechimp
app.MapPost("/api/ets/syncproject", (String projectId) =>
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
}).WithName("SyncProjectTimechimp");

//get employeeids from ets
app.MapGet("/api/ets/employeeids", (String dateString) => new ETSEmployeeHelper(ETSClient).GetEmployeeIdsChangedAfter(DateTime.Parse(dateString))).WithName("GetEmployeeIds");

//get timesids from timechimp
app.MapGet("/api/ets/timeids", (String dateString) => new TimeChimpTimeHelper(TimeChimpClient, ETSClient).GetTimes(DateTime.Parse(dateString))).WithName("GetTimeIds");

//sync time from timechimp to ets
app.MapPost("api/ets/synctime", (String timeId) =>
{
    try
    {
        ETSTimeHelper timeHelperETS = new(ETSClient, TimeChimpClient);
        TimeChimpTimeHelper timeHelperTC = new(TimeChimpClient, ETSClient);

        // Get time from TimeChimp
        timeTimeChimp TCTime = timeHelperTC.GetTime(timeId);

        // Handle when time doesn't exist in TimeChimp
        if (TCTime == null)
        {
            return Results.Problem($"TimeChimp doesn't contain a time with id = {timeId}");
        }

        // Change to ETS class
        timeETS ETSTime = new(TCTime);

        // add time to ETS
        timeHelperETS.addTime(ETSTime);

        List<int> ids = new List<int>();
        ids.Add(Int32.Parse(timeId));

        //change status to invoiced (3)
        timeHelperTC.changeStatus(ids);

        return Results.Ok(timeHelperTC.GetTime(timeId));
    }
    catch (Exception e)
    {
        return Results.Problem(e.Message);
    }
}).WithName("SyncTimeETS");

//sync employee from ets to timechimp
app.MapPost("/api/ets/syncemployee", (String employeeId) =>
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
            return Results.Ok(employeeHelper.UpdateEmployee(TCEmployee));
        }
    }
    catch (Exception e)
    {
        return Results.Problem(e.Message);
    }
}).WithName("SyncEmployeeTimechimp");

//sync mileages from timechimp to ets
app.MapGet("/api/ets/mileage", () =>
{
    try
    {
        //get mileages from ets
        List<mileageETS> mileages = new ETSMileageHelper(ETSClient).GetMileages();

        //get mileages from timechimp
        List<mileageTimeChimp> mileagesTimeChimp = new TimeChimpMileageHelper(TimeChimpClient).GetMileagesByDate(DateTime.Now.AddDays(-7));

        List<int> ids = new List<int>();

        //get projectID
        foreach (mileageTimeChimp mileage in mileagesTimeChimp)
        {
            if (mileage.projectId != null)
            {
                mileage.projectId = new TimeChimpProjectHelper(TimeChimpClient).GetProjectId(mileage.projectId);
                ids.Add(mileage.id);
            }

            EmployeeTimeChimp employee = new TimeChimpEmployeeHelper(TimeChimpClient).GetEmployee(mileage.userId);
            mileage.userId = int.Parse(employee.employeeNumber);
        }


        List<mileageTimeChimp> copyMileages = new List<mileageTimeChimp>(mileagesTimeChimp);
        foreach (mileageTimeChimp mileage in mileagesTimeChimp)
        {
            // check if there is a mileage for a retour of the current mileage
            mileageTimeChimp mileages2 = copyMileages.Find(mileage2 => mileage2.projectId == mileage.projectId && mileage2.userId == mileage.userId && mileage2.distance == mileage.distance && mileage2.fromAddress == mileage.toAddress && mileage2.toAddress == mileage.fromAddress);
            if (mileages2 != null)
            {
                mileage.distance = mileage.distance * 2;
            }
        }

        //change to etsclass
        List<mileageETS> mileagesETS = copyMileages.Select(mileage => new mileageETS(mileage)).ToList();

        //update ets
        foreach (var mileage in mileagesETS)
        {
            var response = new ETSMileageHelper(ETSClient).UpdateMileage(mileage);
        }

        //change status
        var responseStatus = new TimeChimpMileageHelper(TimeChimpClient).changeStatus(ids);

        return Results.Ok(mileagesETS);
    }
    catch (Exception e)
    {
        return Results.Problem(e.Message);
    }
}).WithName("GetMileagesFromETS");

//get mileages from timechimp
app.MapGet("/api/timechimp/mileages", () => new TimeChimpMileageHelper(TimeChimpClient).GetMileages()).WithName("GetMileagesFromTimechimp");

//get uurcodes from timechimp
app.MapGet("/api/timechimp/uurcodes", () => new TimeChimpUurcodeHelper(TimeChimpClient, ETSClient).GetUurcodes()).WithName("GetUurcodes");

//get uurcodes from ets
app.MapGet("/api/ets/uurcodeids", (string dateString) => new ETSUurcodeHelper(ETSClient).GetUurcodes(DateTime.Parse(dateString))).WithName("GetUurcodesFromETS");

//sync uurcodes from ets to timechimp
app.MapPost("/api/ets/syncuurcode", (string uurcodeId) =>
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
}).WithName("UpdateUurcodes");

//get subprojects from ets
app.MapGet("/api/ets/subprojects", (string mainprojectid) => new ETSProjectHelper(ETSClient).GetSubprojects(mainprojectid)).WithName("GetSubprojects");

//get projectusers from timechimp
app.MapGet("/api/timechimp/projectusers", () => new TimeChimpProjectUserHelper(TimeChimpClient).GetProjectUsers()).WithName("GetProjectUsers");

//get projectusers from timechimp by project
app.MapGet("/api/timechimp/projectusers/project", (int projectId) => new TimeChimpProjectUserHelper(TimeChimpClient).GetProjectUsersByProject(projectId)).WithName("GetProjectUsersByProject");

//add projectuser to project in timechimp
app.MapPost("/api/timechimp/projectuserproject", (string projectId) => new TimeChimpProjectUserHelper(TimeChimpClient).AddProjectUserProject(projectId)).WithName("AddProjectUserProject");

//add projectuser to employee in timechimp
app.MapPost("/api/timechimp/projectuseruser", (string userId) => new TimeChimpProjectUserHelper(TimeChimpClient).AddProjectUserEmployee(userId)).WithName("AddProjectUserUser");

app.Run("http://localhost:5001");