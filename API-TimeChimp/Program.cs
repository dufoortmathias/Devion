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
    List<customerTimeChimp> customers = JsonConvert.DeserializeObject<List<customerTimeChimp>>(response.Result);
    return customers;
})
.WithName("GetCustomers");

app.MapPost("/api/devion/customer", (customerTimeChimp customer) =>
{
    //connection with timechimp
    var client = new BearerTokenHttpClient();
    var response = client.PostAsync("customers", JsonConvert.SerializeObject(customer));
    customerTimeChimp customerResponse = JsonConvert.DeserializeObject<customerTimeChimp>(response.Result);
    return customerResponse;
})
.WithName("PostCustomer");

app.MapGet("/api/devion/projects", () => TimeChimpProjectHelper.GetProjects()).WithName("GetProjects");

app.MapPost("/api/devion/project", (ProjectTimeChimp project) => TimeChimpProjectHelper.CreateProject(project)).WithName("CreateProject");

app.MapPut("/api/devion/project", (ProjectTimeChimp project) => TimeChimpProjectHelper.UpdateProject(project)).WithName("UpdateProject");

app.MapGet("api/devion/times", () =>
{
    // connection with timechimp
    var client = new BearerTokenHttpClient();


    // get date from today and 7 days ago
    DateOnly today = DateOnly.FromDateTime(DateTime.Now);
    DateOnly lastWeek = DateOnly.FromDateTime(DateTime.Now.AddDays(-7));

    //get data from timechimp
    var response = client.GetAsync($"time/daterange/{lastWeek.ToString("yyyy-MM-dd")}/{today.ToString("yyyy-MM-dd")}");

    //convert data to timeTimeChimp object
    timeTimeChimp[] times = JsonConvert.DeserializeObject<timeTimeChimp[]>(response.Result);
    Console.WriteLine(times.Length);
    timeTimeChimp[] goedgekeurdeUren = new timeTimeChimp[] { };
    changeRegistrationStatusTimeChimp changeRegistrationStatus = new changeRegistrationStatusTimeChimp();
    List<int> registrationIds = new List<int>();
    foreach (timeTimeChimp time in times)
    {
        //checking uren is goedgekeurd
        if (time.status == 2)
        {
            Console.WriteLine(time.id);
            var id = time.id;
            if (id == null)
            {
                Console.WriteLine("id is null");
            }
            else
            {
                Console.WriteLine(id);
                registrationIds.Add(id);
            }

        }
    }

    //put registrationid in ChangeRegistrationStatusTimeChimp object
    changeRegistrationStatus.registrationIds = registrationIds;
    changeRegistrationStatus.status = 3;
    changeRegistrationStatus.message = "gefactureerd";


    //send to timechimp
    client.PostAsync("time/changestatusintern", JsonConvert.SerializeObject(changeRegistrationStatus));
    //return data
    Console.WriteLine(goedgekeurdeUren.Length);
    return goedgekeurdeUren;
})
.WithName("GetTimesFromLastWeek");

app.MapGet("/api/devion/contacts", () =>
{
    // connection with timechimp
    var client = new BearerTokenHttpClient();
    var response = client.GetAsync("contacts");
    List<contactsTimeChimp> contacts = JsonConvert.DeserializeObject<List<contactsTimeChimp>>(response.Result);
    return contacts;
})
.WithName("GetContacts");

app.MapPost("/api/devion/contact", (contactsTimeChimp contact) =>
{
    //connection with timechimp
    var client = new BearerTokenHttpClient();
    var response = client.PostAsync("contacts", JsonConvert.SerializeObject(contact));
    contactsTimeChimp contactResponse = JsonConvert.DeserializeObject<contactsTimeChimp>(response.Result);
    return contactResponse;
})
.WithName("PostContact");

app.MapPut("/api/devion/contacten", (contactsTimeChimp contact) =>
{
    //connection with timechimp
    var client = new BearerTokenHttpClient();
    var response = client.GetAsync($"contacts/{contact.id}");
    contactsTimeChimp originalContact = JsonConvert.DeserializeObject<contactsTimeChimp>(response.Result);
    //checking with original contact
    if (contact.name != originalContact.name)
    {
        originalContact.name = contact.name;
    }
    else if (contact.jobTitle != originalContact.jobTitle)
    {
        originalContact.jobTitle = contact.jobTitle;
    }
    else if (contact.email != originalContact.email)
    {
        originalContact.email = contact.email;
    }
    else if (contact.phone != originalContact.phone)
    {
        originalContact.phone = contact.phone;
    }
    else if (contact.useForInvoicing != originalContact.useForInvoicing)
    {
        originalContact.useForInvoicing = contact.useForInvoicing;
    }
    else if (contact.active != originalContact.active)
    {
        originalContact.active = contact.active;
    }

    //checking if customerIds are equal
    bool areEqual = originalContact.customerIds.SequenceEqual(contact.customerIds);
    if (!areEqual)
    {
        originalContact.customerIds = contact.customerIds;
    }

    var json = JsonConvert.SerializeObject(originalContact);
    var response2 = client.PutAsync($"contacts", json);
    contactsTimeChimp contactResponse = JsonConvert.DeserializeObject<contactsTimeChimp>(response2.Result);
    return contactResponse;
})
.WithName("PutContact");

app.Run("http://localhost:5001");