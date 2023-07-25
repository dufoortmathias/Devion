using api_Devion.Models;
using Microsoft.AspNetCore.Mvc;

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

app.MapGet("/api/devion/customers", () =>
{
    // connection with timechimp
    var client = new BearerTokenHttpClient();
    var response = client.GetAsync("customers");
    return response;
})
.WithName("GetCustomers");

app.MapPost("/api/devion/customer", (customerTimeChimp customer) =>
{
    //connection with timechimp
    var client = new BearerTokenHttpClient();
    var response = client.PostAsync("customers", JsonConvert.SerializeObject(customer));
    return response;
})
.WithName("PostCustomer");

app.MapGet("/api/devion/projects", () => TimeChimpProjectHelper.GetProjects()).WithName("GetProjects");

app.MapPost("/api/devion/project", (ProjectTimeChimp project) => TimeChimpProjectHelper.CreateProject(project)).WithName("CreateProject");

app.MapPut("/api/devion/project", (ProjectTimeChimp project) => TimeChimpProjectHelper.UpdateProject(project)).WithName("UpdateProject");

app.Run();