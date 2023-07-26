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

app.MapGet("/api/devion/customers", () => TimeChimpCustomerHelper.GetCustomers()).WithName("GetCustomers");

app.MapPost("/api/devion/customer", (customerTimeChimp customer) => TimeChimpCustomerHelper.CreateCustomer(customer)).WithName("CreateCustomer");

app.MapGet("/api/devion/projects", () => TimeChimpProjectHelper.GetProjects()).WithName("GetProjects");

app.MapPost("/api/devion/project", (ProjectTimeChimp project) => TimeChimpProjectHelper.CreateProject(project)).WithName("CreateProject");

app.MapPut("/api/devion/project", (ProjectTimeChimp project) => TimeChimpProjectHelper.UpdateProject(project)).WithName("UpdateProject");

app.MapGet("api/devion/times", () => TimeChimpTimeHelper.GetTimesLastWeek()).WithName("GetTimesFromLastWeek");

app.MapPut("/api/devion/employee", (EmployeeTimeChimp employee) => TimeChimpEmployeeHelper.UpdateEmployee(employee)).WithName("UpdateEmployee");

app.MapPost("/api/devion/employee", (EmployeeTimeChimp employee) => TimeChimpEmployeeHelper.CreateEmployee(employee)).WithName("CreateEmployee");

app.MapGet("/api/devion/employees", () => TimeChimpEmployeeHelper.GetEmployees()).WithName("GetEmployees");

app.MapGet("/api/devion/contacts", () => TimeChimpContactHelper.GetContacts()).WithName("GetContacts");

app.MapPost("/api/devion/contact", (contactsTimeChimp contact) => TimeChimpContactHelper.CreateContact(contact)).WithName("PostContact");

app.MapPut("/api/devion/contacten", (contactsTimeChimp contact) => TimeChimpContactHelper.UpdateContact(contact)).WithName("PutContact");

app.Run();