using api_Devion.Models;

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

string api_key_timechimp = "3Hr14yu7DrW4R7YcRfSQDTjBldTpvRuqvzoUG60uN_Sqyl2dlBZakWwyIfZsH4GKeSPAkj1sp8y6zKSJhgQ8pXhAFukK9VB1AjcU97NVkqj8LO1nGof_9dy4u4Ui4EBgnt3Nmyu9tU-ia0cYcwqZJnlMDP-YunXu9hH-230PnlklEy-nHOZ7a7bORvJ0zYMM_U961cfJNeAXH39kFIDfOj9KtnGGZbgwfvDfm6KapW-uoT7ehUN1lLLVhXSTlQO1SNjRkDN15ZRLA9veYydybmizGIQtMVIxvZ726G3GCGpj4nvx";
string baseUrl = "https://api.timechimp.com/v1/";

app.MapGet("/api/devion/customers", () =>
{
    // connection with timechimp
    var client = new BearerTokenHttpClient(baseUrl, api_key_timechimp);
    var response = client.GetAsync("customers");
    return response;
})
.WithName("GetCustomers");

app.MapPost("/api/devion/customer", (customerTimeChimp customer) =>
{
    //connection with timechimp
    var client = new BearerTokenHttpClient(baseUrl, api_key_timechimp);
    var response = client.PostAsync("customers", JsonConvert.SerializeObject(customer));
    return response;
})
.WithName("PostCustomer");

app.MapGet("/api/devion/projects", () =>
{
    //connection with timechimp
    var client = new BearerTokenHttpClient(baseUrl, api_key_timechimp);
    var response = client.GetAsync("projects");
    ProjectTimeChimp[] projects = JsonConvert.DeserializeObject<ProjectTimeChimp[]>(response.Result);
    return projects;
})
.WithName("GetProjects");

app.MapPost("/api/devion/project", (ProjectTimeChimp project) =>
{
    //connection with timechimp
    var client = new BearerTokenHttpClient(baseUrl, api_key_timechimp);
    var response = client.PostAsync("projects", JsonConvert.SerializeObject(project));
    return response;
})
.WithName("CreateProject");

app.MapPut("/api/devion/project", (ProjectTimeChimp project) =>
{
    //connection with timechimp
    var client = new BearerTokenHttpClient(baseUrl, api_key_timechimp);
    var response = client.PutAsync("projects", JsonConvert.SerializeObject(project));
    return response;
})
.WithName("UpdateProject");


app.Run();