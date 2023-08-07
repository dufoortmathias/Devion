var builder = WebApplication.CreateBuilder(args);

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

app.MapGet("/api/timechimp/customers", () => TimeChimpCustomerHelper.GetCustomers()).WithName("GetCustomers");

app.MapPost("/api/timechimp/customer", (customerTimeChimp customer) => TimeChimpCustomerHelper.CreateCustomer(customer)).WithName("CreateCustomer");

app.MapGet("/api/timechimp/projects", () => TimeChimpProjectHelper.GetProjects()).WithName("GetProjects");

app.MapPost("/api/timechimp/project", (ProjectTimeChimp project) => TimeChimpProjectHelper.CreateProject(project)).WithName("CreateProject");

app.MapPut("/api/timechimp/project", (ProjectTimeChimp project) => TimeChimpProjectHelper.UpdateProject(project)).WithName("UpdateProject");

app.MapGet("api/timechimp/times", () => TimeChimpTimeHelper.GetTimesLastWeek()).WithName("GetTimesFromLastWeek");

app.MapPut("/api/timechimp/employee", (EmployeeTimeChimp employee) => TimeChimpEmployeeHelper.UpdateEmployee(employee)).WithName("UpdateEmployee");

app.MapPost("/api/timechimp/employee", (EmployeeTimeChimp employee) => TimeChimpEmployeeHelper.CreateEmployee(employee)).WithName("CreateEmployee");

app.MapGet("/api/timechimp/employees", () => TimeChimpEmployeeHelper.GetEmployees()).WithName("GetEmployees");

app.MapGet("/api/timechimp/contacts", () => TimeChimpContactHelper.GetContacts()).WithName("GetContacts");

app.MapPost("/api/timechimp/contact", (contactsTimeChimp contact) => TimeChimpContactHelper.CreateContact(contact)).WithName("PostContact");

app.MapPut("/api/timechimp/contacten", (contactsTimeChimp contact) => TimeChimpContactHelper.UpdateContact(contact)).WithName("PutContact");

app.MapGet("/api/timechimp/mileage", () => TimeChimpMileageHelper.GetMileages()).WithName("GetMileages");

app.MapGet("/api/ets/customerids", (String dateString) => ETSCustomerHelper.GetCustomerIdsChangedAfter(DateTime.Parse(dateString))).WithName("GetCustomerIds");

app.MapPost("/api/ets/synccustomer", (String customerId) =>
{
    CustomersETS ETSCustomer = ETSCustomerHelper.GetCustomer(customerId);

    // Handle when customer doesn't exist in ETS
    if (ETSCustomer == null)
    {
        return Results.Problem($"ETS doesn't contain a customer with id = {customerId}");
    }

    customerTimeChimp TCCustomer = new(ETSCustomer);

    if (TimeChimpCustomerHelper.CustomerExists(customerId))
    {
        return Results.Ok(TimeChimpCustomerHelper.UpdateCustomer(TCCustomer));
    }
    else
    {
        return Results.Ok(TimeChimpCustomerHelper.CreateCustomer(TCCustomer));
    }
}).WithName("SyncCustomerTimechimp");

app.MapGet("/api/ets/contactids", (String dateString) => ETSContactHelper.GetContactIdsChangedAfter(DateTime.Parse(dateString))).WithName("GetContactIds");

app.MapPost("/api/ets/synccontact", (Int32 contactId) =>
{
    contactsETS ETSContact = ETSContactHelper.GetContact(contactId);

    // Handle when contact doesn't exist in ETS
    if (ETSContact == null)
    {
        return Results.Problem($"ETS doesn't contain a contact with id = {contactId}");
    }

    contactsTimeChimp TCContact = new(ETSContact);

    if (TimeChimpContactHelper.ContactExists(ETSContact))
    {
        return Results.Ok(TimeChimpContactHelper.UpdateContact(TCContact));
    }
    else
    {
        return Results.Ok(TimeChimpContactHelper.CreateContact(TCContact));
    }
}).WithName("SyncContactTimechimp");

app.MapGet("/api/ets/projectids", (String dateString) => ETSProjectHelper.GetProjectIdsChangedAfter(DateTime.Parse(dateString))).WithName("GetProjectIds");

app.MapPost("/api/ets/syncproject", (String projectId) =>
{
    ProjectETS ETSProject = ETSProjectHelper.GetProject(projectId);

    // Handle when project doesn't exist in ETS
    if (ETSProject == null)
    {
        return Results.Problem($"ETS doesn't contain a project with id = {projectId}");
    }
    else if (ETSProject.PR_KLNR == null)
    {
        return Results.Problem($"The ETS record for project with id = {projectId} doesn't has a customernumber");
    }

    ProjectTimeChimp TCProject = new(ETSProject);


    ProjectTimeChimp createdMainProject;
    if (TimeChimpProjectHelper.ProjectExists(projectId))
    {
        createdMainProject = TimeChimpProjectHelper.UpdateProject(TCProject);
    }
    else
    {
        createdMainProject = TimeChimpProjectHelper.CreateProject(TCProject);
        createdMainProject = TimeChimpProjectHelper.UpdateProject(TCProject);
    }

    List<SubprojectETS> ETSSubprojects = ETSProjectHelper.GetSubprojects(projectId);
    foreach (SubprojectETS ETSSubproject in ETSSubprojects)
    {
        ProjectTimeChimp TCSubproject = new(ETSSubproject, TCProject);
        TCSubproject.mainProjectId = TCProject.id;

        if (TimeChimpProjectHelper.ProjectExists(TCSubproject.code))
        {
            TimeChimpProjectHelper.UpdateProject(TCSubproject);
        }
        else
        {
            TimeChimpProjectHelper.CreateProject(TCSubproject);
            TimeChimpProjectHelper.UpdateProject(TCSubproject);
        }
    }

    return Results.Ok(TimeChimpProjectHelper.GetProject(createdMainProject.id.Value));
}).WithName("SyncProjectTimechimp");

app.MapGet("/api/ets/employeeids", (String dateString) => ETSEmployeeHelper.GetEmployeeIdsChangedAfter(DateTime.Parse(dateString))).WithName("GetEmployeeIds");
app.MapGet("/api/ets/times", () => ETSTimeHelper.GetTime()).WithName("GetTimesFromETS");

app.MapPost("/api/ets/syncemployee", (String employeeId) =>
{
    EmployeeETS ETSEmployee = ETSEmployeeHelper.GetEmployee(employeeId);

    // Handle when contact doesn't exist in ETS
    if (ETSEmployee == null)
    {
        return Results.Problem($"ETS doesn't contain an employee with id = {employeeId}");
    }

    EmployeeTimeChimp TCEmployee = new(ETSEmployee);

    if (TimeChimpEmployeeHelper.EmployeeExists(employeeId))
    {
        return Results.Ok(TimeChimpEmployeeHelper.UpdateEmployee(TCEmployee));
    }
    else
    {
        if (TCEmployee.userName == null)
        {
            return Results.Problem($"Can't create the employee {TCEmployee.displayName} without an emailaddress");
        }

        EmployeeTimeChimp employee = TimeChimpEmployeeHelper.CreateEmployee(TCEmployee);
        TCEmployee.id = employee.id;
        return Results.Ok(TimeChimpEmployeeHelper.UpdateEmployee(TCEmployee));
    }
}).WithName("SyncEmployeeTimechimp");

app.MapGet("/api/ets/times", () => ETSTimeHelper.addTimes()).WithName("GetTimesFromETS");

app.MapGet("/api/ets/mileage", () =>
{
    List<mileageETS> mileages = ETSMileageHelper.GetMileages();
    List<mileageTimeChimp> mileagesTimeChimp = TimeChimpMileageHelper.GetMileagesByDate(DateTime.Now.AddDays(-7));

    List<int> ids = new List<int>();

    //get projectID
    foreach (mileageTimeChimp mileage in mileagesTimeChimp)
    {
        if (mileage.projectId != null)
        {
            mileage.projectId = TimeChimpProjectHelper.GetProjectId(mileage.projectId);
            ids.Add(mileage.id);
        }

        EmployeeTimeChimp employee = TimeChimpEmployeeHelper.GetEmployee(mileage.userId);
        mileage.userId = Int32.Parse(employee.employeeNumber);
    }

    List<mileageTimeChimp> copyMileages = new List<mileageTimeChimp>(mileagesTimeChimp);
    foreach (mileageTimeChimp mileage in mileagesTimeChimp)
    {
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
        var response = ETSMileageHelper.UpdateMileage(mileage);
    }

    //change status
    var responseStatus = TimeChimpMileageHelper.changeStatus(ids);

    return mileagesETS;
}).WithName("GetMileagesFromETS");

app.MapGet("/api/timechimp/mileages", () => TimeChimpMileageHelper.GetMileages()).WithName("GetMileagesFromTimechimp");

app.MapGet("/api/timechimp/uurcodes", () => TimeChimpUurcodeHelper.GetUurcodes()).WithName("GetUurcodes");

app.MapGet("/api/ets/uurcodes", () => ETSUurcodeHelper.GetUurcodes()).WithName("GetUurcodesFromETS");

app.MapPost("/api/timechimp/updateUurcodes", () => TimeChimpUurcodeHelper.UpdateUurcodes()).WithName("UpdateUurcodes");

app.MapGet("/api/ets/subprojects", (string mainprojectid) => ETSProjectHelper.GetSubprojects(mainprojectid)).WithName("GetSubprojects");

app.MapGet("/api/timechimp/projectusers", () => TimeChimpProjectUserHelper.GetProjectUsers()).WithName("GetProjectUsers");

app.MapGet("/api/timechimp/projectusers/project", (int projectId) => TimeChimpProjectUserHelper.GetProjectUsersByProject(projectId)).WithName("GetProjectUsersByProject");

app.MapPost("/api/timechimp/projectuserproject", (string projectId) => TimeChimpProjectUserHelper.AddProjectUserProject(projectId)).WithName("AddProjectUserProject");

app.MapPost("/api/timechimp/projectuseruser", (string userId) => TimeChimpProjectUserHelper.AddProjectUserEmployee(userId)).WithName("AddProjectUserUser");

app.Run();