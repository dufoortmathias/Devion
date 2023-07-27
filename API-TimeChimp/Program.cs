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

app.MapGet("/api/ets/customers", () => ETSCustomerHelper.GetCustomers()).WithName("GetCustomersETS");

app.MapGet("/api/ets/contacts", () => ETSContactHelper.GetContacts()).WithName("GetContactsETS");

app.MapGet("/api/ets/employees", () =>
{
    List<EmployeeETS> employees = ETSEmployeeHelper.GetEmployees();
    List<EmployeeTimeChimp> employeesTimeChimp = employees.Select(employee => new EmployeeTimeChimp(employee)).ToList();
    foreach (EmployeeTimeChimp employee in employeesTimeChimp)
    {
        TimeChimpEmployeeHelper.CreateEmployee(employee);
    }
    return employees;

}).WithName("GetEmployeesETS");

app.Run("http://localhost:5001");