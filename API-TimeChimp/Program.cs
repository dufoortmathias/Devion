WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ConfigurationManager config = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder => builder.AllowAnyOrigin());
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAllOrigins");
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseHttpsRedirection();



List<string> companies = new();

int companyIndex = -1;
while (config[$"Companies:{++companyIndex}:Name"] != null)
{
    WebClient TCClient = new(config["TimeChimpBaseURL"], config[$"Companies:{companyIndex}:TimeChimpToken"]);
    FirebirdClientETS ETSClient = new(config["ETSServer"], config[$"Companies:{companyIndex}:ETSUser"], config[$"Companies:{companyIndex}:ETSPassword"], config[$"Companies:{companyIndex}:ETSDatabase"]);

    string company = config[$"Companies:{companyIndex}:Name"];
    companies.Add(company);

    //get customers from timechimp
    //app.MapGet($"/api/{company.ToLower()}/timechimp/customers", () => { try { return Results.Ok(new TimeChimpCustomerHelper(TCClient).GetCustomers()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetCustomers");

    //create customer in timechimp
    //app.MapPost($"/api/{company.ToLower()}/timechimp/customer", (CustomerTimeChimp customer) => { try { return Results.Ok(new TimeChimpCustomerHelper(TCClient).CreateCustomer(customer)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}CreateCustomer");

    //get projects from timechimp
    //app.MapGet($"/api/{company.ToLower()}/timechimp/projects", () => { try { return Results.Ok(new TimeChimpProjectHelper(TCClient).GetProjects()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetProjects");

    //create project in timechimp
    //app.MapPost($"/api/{company.ToLower()}/timechimp/project", (ProjectTimeChimp project) => { try { return Results.Ok(new TimeChimpProjectHelper(TCClient).CreateProject(project)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}CreateProject");

    //update project in timechimp
    //app.MapPut($"/api/{company.ToLower()}/timechimp/project", (ProjectTimeChimp project) => { try { return Results.Ok(new TimeChimpProjectHelper(TCClient).UpdateProject(project)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}UpdateProject");

    //get times from timechimp last week
    //app.MapGet($"/api/{company.ToLower()}/timechimp/times", () => { try { return Results.Ok(new TimeChimpTimeHelper(TCClient, ETSClient).GetTimes(DateTime.Now.AddDays(-7))); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetTimesFromLastWeek");

    //update employee in timechimp
    //app.MapPut($"/api/{company.ToLower()}/timechimp/employee", (EmployeeTimeChimp employee) => { try { return Results.Ok(new TimeChimpEmployeeHelper(TCClient).UpdateEmployee(employee)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}UpdateEmployee");

    //create employee in timechimp
    //app.MapPost($"/api/{company.ToLower()}/timechimp/employee", (EmployeeTimeChimp employee) => { try { return Results.Ok(new TimeChimpEmployeeHelper(TCClient).CreateEmployee(employee)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}CreateEmployee");

    //get employees from timechimp
    //app.MapGet($"/api/{company.ToLower()}/timechimp/employees", () => { try { return Results.Ok(new TimeChimpEmployeeHelper(TCClient).GetEmployees()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetEmployees");

    //get contacts from timechimp
    //app.MapGet($"/api/{company.ToLower()}/timechimp/contacts", () => { try { return Results.Ok(new TimeChimpContactHelper(TCClient).GetContacts()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetContacts");

    //create contact in timechimp
    //app.MapPost($"/api/{company.ToLower()}/timechimp/contact", (ContactTimeChimp contact) => { try { return Results.Ok(new TimeChimpContactHelper(TCClient).CreateContact(contact)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}PostContact");

    //update contact in timechimp
    //app.MapPut($"/api/{company.ToLower()}/timechimp/contacten", (ContactTimeChimp contact) => { try { return Results.Ok(new TimeChimpContactHelper(TCClient).UpdateContact(contact)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}PutContact");

    //get mileages from timechimp and send the to ets
    //app.MapGet($"/api/{company.ToLower()}/timechimp/mileage", () => { try { return Results.Ok(new TimeChimpMileageHelper(TCClient).GetMileages()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetMileages");

    //get mileages from timechimp
    //app.MapGet($"/api/{company.ToLower()}/timechimp/mileages", () => { try { return Results.Ok(new TimeChimpMileageHelper(TCClient).GetMileages()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetMileagesFromTimechimp");

    //get uurcodes from timechimp
    //app.MapGet($"/api/{company.ToLower()}/timechimp/uurcodes", () => { try { return Results.Ok(new TimeChimpUurcodeHelper(TCClient, ETSClient).GetUurcodes()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetUurcodes");

    //get subprojects from ets
    //app.MapGet($"/api/{company.ToLower()}/ets/subprojects", (string mainprojectid) => { try { return Results.Ok(new ETSProjectHelper(ETSClient).GetSubprojects(mainprojectid)); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetSubprojects");

    //get projectusers from timechimp
    //app.MapGet($"/api/{company.ToLower()}/timechimp/projectusers", () => { try { return Results.Ok(new TimeChimpProjectUserHelper(TCClient).GetProjectUsers()); } catch (Exception e) { return Results.Problem(e.Message); } }).WithName($"{company}GetProjectUsers");

    //get customerids from ets
    app.MapGet($"/api/{company.ToLower()}/ets/customerids", (string dateString) =>
    {
        try
        {
            return Results.Ok(new ETSCustomerHelper(ETSClient).GetCustomerIdsChangedAfter(DateTime.Parse(dateString)));
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}GetCustomerIds").WithTags(company);

    //sync customer from ets to timechimp
    app.MapPost($"/api/{company.ToLower()}/ets/synccustomer", (string customerId) =>
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
            return customerHelper.CustomerExists(customerId)
                ? Results.Ok(customerHelper.UpdateCustomer(TCCustomer))
                : Results.Ok(customerHelper.CreateCustomer(TCCustomer));
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}SyncCustomerTimechimp").WithTags(company);

    //get contactids from ets
    app.MapGet($"/api/{company.ToLower()}/ets/contactids", (string dateString) =>
    {
        try
        {
            return Results.Ok(new ETSContactHelper(ETSClient).GetContactIdsChangedAfter(DateTime.Parse(dateString)));
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}GetContactIds").WithTags(company);

    //sync contact from ets to timechimp
    app.MapPost($"/api/{company.ToLower()}/ets/synccontact", (int contactId) =>
    {
        try
        {
            //get contact from ets
            ContactETS ETSContact = new ETSContactHelper(ETSClient).GetContact(contactId);

            // Handle when contact doesn't exist in ETS
            if (ETSContact == null)
            {
                throw new Exception($"ETS doesn't contain a contact with id = {contactId}");
            }

            CustomerTimeChimp customer = new TimeChimpCustomerHelper(TCClient).GetCustomers().Find(c => c.relationId != null && c.relationId.Equals(ETSContact.CO_KLCOD)) ?? throw new Exception($"Customer with number = {ETSContact.CO_KLCOD} doesn't exist in TimeChimp");
            int customerId = customer.id.Value;

            //change to timechimp class
            ContactTimeChimp TCContact = new(ETSContact, customerId);

            TimeChimpContactHelper contactHelper = new(TCClient);

            //check if contact exists in timechimp
            return contactHelper.ContactExists(ETSContact)
                ? Results.Ok(contactHelper.UpdateContact(TCContact))
                : Results.Ok(contactHelper.CreateContact(TCContact));
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}SyncContactTimechimp").WithTags(company); ;

    //get uurcodes from ets
    app.MapGet($"/api/{company.ToLower()}/ets/uurcodeids", (string dateString) =>
    {
        try
        {
            return Results.Ok(new ETSUurcodeHelper(ETSClient).GetUurcodes(DateTime.Parse(dateString)));
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}GetUurcodesFromETS").WithTags(company);

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

            TimeChimpUurcodeHelper uurcodeHelper = new(TCClient);

            //check if uurcode exists in timechimp
            return uurcodeHelper.uurcodeExists(uurcodeId)
                ? Results.Ok(uurcodeHelper.UpdateUurcode(TCUurcode))
                : Results.Ok(uurcodeHelper.CreateUurcode(TCUurcode));
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}UpdateUurcodes").WithTags(company);

    //get employeeids from ets
    app.MapGet($"/api/{company.ToLower()}/ets/employeeids", (string dateString, string teamName) =>
    {
        try
        {
            return Results.Ok(new ETSEmployeeHelper(ETSClient).GetEmployeeIdsChangedAfter(DateTime.Parse(dateString), teamName));
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}GetEmployeeIds").WithTags(company);

    //sync employee from ets to timechimp
    app.MapPost($"/api/{company.ToLower()}/ets/syncemployee", (string employeeId) =>
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


            TimeChimpEmployeeHelper employeeHelper = new(TCClient);

            //determine role by most used role for all users except admins/managers
            int roleId = employeeHelper.GetEmployees()
                .Where(e => e.roleId > 4 || e.roleId == 1)
                .GroupBy(e => e.roleId.Value)
                .Select(g => new
                {
                    RoleId = g.Key,
                    Count = g.Count()
                })
                .MaxBy(o => o.Count)?.RoleId ?? 1;

            //change to timechimp class
            EmployeeTimeChimp TCEmployee = new(ETSEmployee, roleId);

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
                _ = new TimeChimpProjectUserHelper(TCClient).AddAllProjectUserForEmployee(employee.id.Value);

                return Results.Ok(employee);
            }
        }
        catch (Exception exception)
        {
            return Results.Problem(exception.Message);
        }

    }).WithName($"{company}SyncEmployeeTimechimp").WithTags(company);

    //get projectids from ets
    app.MapGet($"/api/{company.ToLower()}/ets/projectids", (string dateString) =>
    {
        try
        {
            return Results.Ok(new ETSProjectHelper(ETSClient).GetProjectIdsChangedAfter(DateTime.Parse(dateString)));
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}GetProjectIds").WithTags(company);

    //sync project from ets to timechimp
    app.MapPost($"/api/{company.ToLower()}/ets/syncproject", (string projectId) =>
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

            // Change to TimeChimp class
            ProjectTimeChimp TCProject = new(ETSProject);

            CustomerTimeChimp customer;
            // Find customer id TimeChimpETSContact.CO_KLCOD
            if (ETSProject.PR_KLNR == null)
            {
                customer = new TimeChimpCustomerHelper(TCClient).GetCustomers().Find(c => c.intern) ?? throw new Exception($"The ETS record for project with id = {projectId} has no customernumber, internal customer is still archived in TimeChimp!");
            }
            else
            {
                customer = new TimeChimpCustomerHelper(TCClient).GetCustomers().Find(c => c.relationId != null && c.relationId.Equals(ETSProject.PR_KLNR)) ?? throw new Exception($"No timechimp cutomer found with id = {ETSProject.PR_KLNR}");
            }
            TCProject.customerId = customer.id.Value;

            ProjectTimeChimp mainProject = projectHelperTC.FindProject(projectId) ?? projectHelperTC.CreateProject(TCProject);

            List<string> errorMessages = new();
            double totalBudgetHours = 0;
            List<UurcodeTimeChimp> uurcodes = new TimeChimpUurcodeHelper(TCClient).GetUurcodes();
            // get subprojects from ETS
            List<SubprojectETS> ETSSubprojects = projectHelperETS.GetSubprojects(projectId);
            foreach (SubprojectETS ETSSubproject in ETSSubprojects.Where(subProject => !subProject.SU_SUB.StartsWith('2'))) //  only iterate subprojects with ids from [0000, 2000[ en [3000, ...]
            {
                // Change to TimeChimp class
                ProjectTimeChimp TCSubproject = new(ETSSubproject, mainProject)
                {
                    mainProjectId = mainProject.id
                };

                ProjectTimeChimp subProject = projectHelperTC.FindProject(TCSubproject.code) ?? projectHelperTC.CreateProject(TCSubproject);

                TCSubproject.id = subProject.id;
                subProject = projectHelperTC.UpdateProject(TCSubproject);

                //update budgethours for each projecttask in timeChimp
                List<ProjectTaskETS> projectTasksETS = new ETSUurcodeHelper(ETSClient).GetUurcodesSubproject(ETSProject.PR_NR, ETSSubproject.SU_SUB);
                foreach (ProjectTaskETS projectTaskETS in projectTasksETS)
                {
                    if (string.IsNullOrEmpty(projectTaskETS.VO_PROJ?.Trim()))
                    {
                        errorMessages.Add($"Subproject {subProject.code} field VO_PROJ is empty for record {projectTaskETS.VO_ID} in table J2W_VOPX");
                    }
                    else if (string.IsNullOrEmpty(projectTaskETS.VO_SUBPROJ?.Trim()))
                    {
                        errorMessages.Add($"Subproject {subProject.code} field VO_PROJ is empty for record {projectTaskETS.VO_ID} in table J2W_VOPX");
                    }
                    else if (string.IsNullOrEmpty(projectTaskETS.VO_UUR?.Trim()))
                    {
                        errorMessages.Add($"Subproject {subProject.code} field VO_UUR is empty for record {projectTaskETS.VO_ID} in table J2W_VOPX");
                    }
                    else if (projectTaskETS.VO_AANT == null)
                    {
                        errorMessages.Add($"Subproject {subProject.code} field VO_AANT is null for record {projectTaskETS.VO_ID} in table J2W_VOPX");
                    }
                    else
                    {
                        int taskId = uurcodes.Find(u => u.code.Equals(projectTaskETS.VO_UUR)).id;
                        ProjectTaskTimechimp projectTaskTimechimp = subProject.projectTasks.Find(p => p.taskId.Equals(taskId));

                        projectTaskTimechimp.budgetHours = projectTaskETS.VO_AANT;
                        totalBudgetHours += projectTaskETS.VO_AANT.Value;

                        TCClient.PutAsync("v1/projecttasks", JsonTool.ConvertFrom(projectTaskTimechimp));
                    }
                }
            }

            // update mainproject
            TCProject.id = mainProject.id;
            TCProject.budgetHours = totalBudgetHours;
            mainProject = projectHelperTC.UpdateProject(TCProject);


            if (errorMessages.Count > 0)
            {
                throw new Exception(string.Join(", ", errorMessages));
            }

            return Results.Ok(mainProject);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}SyncProjectTimechimp").WithTags(company);

    //get timesids from timechimp
    app.MapGet($"/api/{company.ToLower()}/ets/timeids", (string dateString) =>
    {
        try
        {
            return Results.Ok(new TimeChimpTimeHelper(TCClient, ETSClient).GetTimes(DateTime.Parse(dateString)));
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}GetTimeIds").WithTags(company);

    //sync time from timechimp to ets
    app.MapPost($"/api/{company.ToLower()}/ets/synctime", (int timeId) =>
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
    }).WithName($"{company}SyncTimeETS").WithTags(company);

    //get mileageids from timechimp that were changed after specific time
    app.MapGet($"/api/{company.ToLower()}/ets/mileageids", (string dateString) =>
    {
        try
        {
            return Results.Ok(new TimeChimpMileageHelper(TCClient).GetApprovedMileageIdsByDate(DateTime.Parse(dateString)));
        }
        catch (Exception exception)
        {
            return Results.Problem(exception.Message);
        }
    }).WithName($"{company}GetMileageIds").WithTags(company);

    //sync mileages from timechimp to ets
    app.MapPost($"/api/{company.ToLower()}/ets/syncmileage", (int mileageId) =>
    {
        try
        {
            MileageTimeChimp mileage = new TimeChimpMileageHelper(TCClient).GetMileage(mileageId);

            if (mileage.status == 3)
            {
                throw new Exception($"Mileage with id ({mileageId}) already invoiced");
            }

            string projectNumber = new TimeChimpProjectHelper(TCClient).GetProject(mileage.projectId).code;
            string employeeNumber = new TimeChimpEmployeeHelper(TCClient).GetEmployee(mileage.userId).employeeNumber;

            MileageETS mileageETS = new(mileage, projectNumber, employeeNumber);
            MileageETS response = new ETSMileageHelper(ETSClient).UpdateMileage(mileageETS);

            //change status
            MileageTimeChimp responseStatus = new TimeChimpMileageHelper(TCClient).changeStatus(mileageId);

            return Results.Ok(response);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}GetMileagesFromETS").WithTags(company);

    //get numbers for all the open purchase orders
    app.MapGet($"/api/{company.ToLower()}/ets/openpurchaseorderids", () =>
    {
        try
        {
            List<PurchaseOrderHeaderETS> purchaseOrders = new ETSPurchaseOrderHelper(ETSClient).GetOpenPurchaseOrders();
            var purchaseOrderIds = purchaseOrders.Select(p => p.FH_BONNR).Distinct();
            return Results.Ok(purchaseOrderIds);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}GetOpenPurchaseOrderIds").WithTags(company);

    //get details about specific purchase order
    app.MapGet($"/api/{company.ToLower()}/ets/purchaseorder", (string id) =>
    {
        try
        {
            List<PurchaseOrderDetailETS> purchaseOrders = new ETSPurchaseOrderHelper(ETSClient).GetPurchaseOrderDetails(id);

            //convert to a dictionary for the web
            Dictionary<string, object> result = new()
                {
                    {"bonNummer", id},
                    {"artikels", new List<Dictionary<string, object>>()},
                    {"klant", purchaseOrders.FirstOrDefault()?.KLANTNAAM},
                    {"project", purchaseOrders.FirstOrDefault()?.FD_PROJ},
                    {"subproject", purchaseOrders.FirstOrDefault()?.FD_SUBPROJ}
                };
            foreach (PurchaseOrderDetailETS purchaseOrder in purchaseOrders.Where(p => p.FD_ARTNR != null))
            {
                ((List<Dictionary<string, object>>)result["artikels"]).Add(new()
                    {
                        {"artikelNummer", purchaseOrder.FD_ARTNR},
                        {"omschrijving", purchaseOrder.FD_OMS},
                        {"aantal", purchaseOrder.FD_AANTAL.Value},
                        {"leverancier", purchaseOrder.LV_NAM}
                    });
            }

            return Results.Ok(result);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}GetPurchaseOrder").WithTags(company);

    //returns file information for each supplier about file needed for order 
    app.MapGet($"/api/{company.ToLower()}/ets/createpurchasefile", (string id) =>
    {
        try
        {
            ETSPurchaseOrderHelper helper = new(ETSClient);

            List<FileContentResult> fileContents = new();
            helper.GetPurchaseOrderDetails(id)
                .Where(po => po.LV_COD != null)
                .GroupBy(po => po.LV_COD)
                .Select(g => g.ToList())
                .ToList()
                .ForEach(purchaseOrders =>
                {
                    if (purchaseOrders.First().LV_NAM.ToLower().Contains("cebeo"))
                    {
                        fileContents.Add(helper.CreateFileCebeo(purchaseOrders, config));
                    }
                    else
                    {
                        fileContents.Add(helper.CreateCSVFile(purchaseOrders));
                    }
                });

            return Results.Ok(fileContents);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}CreatePurchaseFile").WithTags(company);

    app.MapGet($"/api/{company.ToLower()}/cebeo/articles", () =>
    {
        try
        {
            List<string> articles = new ETSArticleHelper(ETSClient, config).GetAriclesCebeo();

            return Results.Ok(articles);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}GetArticles").WithTags(company);

    app.MapGet($"/api/{company.ToLower()}/cebeo/updatearticleprice", (string articleNumberETS, float maxPriceDiff) =>
    {
        try
        {
            ETSArticleHelper helper = new(ETSClient, config);

            string articleReference = helper.GetArticle(articleNumberETS).ART_LEVREF ?? throw new Exception($"Article in ETS with number = {articleNumberETS}, has no supplier reference number");

            string articleNumberCebeo = helper.GetArticleNumberCebeo(articleReference) ?? throw new Exception($"Cebeo has no article with reference = {articleReference}");

            float newPrice = helper.GetArticlePriceCebeo(articleNumberCebeo) ?? throw new Exception($"Cebeo has no article with number = {articleNumberCebeo}");

            ArticleETS article = helper.UpdateArticlePriceETS(articleNumberETS, newPrice, maxPriceDiff);

            return Results.Ok(article.ART_AANKP == newPrice ? $"Price updated to {article.ART_AANKP}" : $"Price not updated, price diff is {Math.Abs(article.ART_AANKP.Value - newPrice) / article.ART_AANKP * 100}%");
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}UpdateArticlePrice").WithTags(company);
}

app.MapGet("/api/companies", () => Results.Ok(companies)).WithName($"GetCompanyNames");

app.Run();