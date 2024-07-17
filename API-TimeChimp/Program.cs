using Api.Devion.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ConfigurationManager config = builder.Configuration;

Config config1 = new Config(config);
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigins", builder =>
    {
        builder.WithOrigins("*");
        builder.WithHeaders("*");
        builder.AllowAnyMethod();
    });
});

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("AllowOrigins");
//app.UseHttpsRedirection();



List<string> companies = new();

Dictionary<string, Dictionary<string, float>> priceSettings = new();
Dictionary<string, Dictionary<string, string>> bewerkingenSettings = new();

int companyIndex = -1;
int syncDevion = 0;
int syncMetabil = 0;
while (config[$"Companies:{++companyIndex}:Name"] != null)
{
    //create clients
    WebClient TCClient = new(config["TimeChimpBaseURL"], config[$"Companies:{companyIndex}:TimeChimpToken"]);
    FirebirdClientETS ETSClient = new(config["ETSServer"], config[$"Companies:{companyIndex}:ETSUser"], config[$"Companies:{companyIndex}:ETSPassword"], config[$"Companies:{companyIndex}:ETSDatabase"]);

    //create helpers
    CebeoArticleHelper articleHelperCebeo = new(config);

    ETSArticleHelper articleHelperETS = new(ETSClient);
    ETSContactHelper contactHelperETS = new(ETSClient);
    ETSCustomerHelper customerHelperETS = new(ETSClient);
    ETSEmployeeHelper employeeHelperETS = new(ETSClient);
    ETSMileageHelper mileageHelperETS = new(ETSClient);
    ETSProjectHelper projectHelperETS = new(ETSClient);
    ETSPurchaseOrderHelper purchaseOrderHelperETS = new(ETSClient);
    ETSSupplierHelper supplierHelperETS = new(ETSClient);
    ETSTimeHelper timeHelperETS = new(ETSClient, TCClient);
    ETSUurcodeHelper uurcodeHelperETS = new(ETSClient);
    ETSProjVoortgangHelper projVoortgangHelperETS = new(ETSClient);
    ETSItemHelper itemHelperETS = new(ETSClient);

    TimeChimpContactHelper contactHelperTC = new(TCClient);
    TimeChimpCustomerHelper customerHelperTC = new(TCClient);
    TimeChimpEmployeeHelper employeeHelperTC = new(TCClient);
    TimeChimpMileageHelper mileageHelperTC = new(TCClient);
    TimeChimpProjectHelper projectHelperTC = new(TCClient);
    TimeChimpProjectUserHelper projectUserHelperTC = new(TCClient);
    TimeChimpProjectTaskHelper projectTaskHelperTC = new(TCClient);
    TimeChimpTimeHelper timeHelperTC = new(TCClient, ETSClient);
    TimeChimpUurcodeHelper uurcodeHelperTC = new(TCClient);
    VehicleHelper vehicleHelper = new(TCClient);

    //get companies from config
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
            return Results.Ok(customerHelperETS.GetCustomerIdsChangedAfter(DateTime.Parse(dateString)));
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
            CustomerETS? ETSCustomer = customerHelperETS.GetCustomer(customerId);

            // Handle when customer doesn't exist in ETS
            if (ETSCustomer == null)
            {
                return Results.Problem($"ETS doesn't contain a customer with id = {customerId}");
            }

            //change to timechimp class
            CustomerTimeChimp TCCustomer = new(ETSCustomer);

            //check if customer exists in timechimp
            return customerHelperTC.CustomerExists(customerId)
                ? Results.Ok(customerHelperTC.UpdateCustomer(TCCustomer))
                : Results.Ok(customerHelperTC.CreateCustomer(TCCustomer));
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
            return Results.Ok(contactHelperETS.GetContactIdsChangedAfter(DateTime.Parse(dateString)));
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
            ContactETS ETSContact = contactHelperETS.GetContact(contactId) ?? throw new Exception($"ETS doesn't contain a contact with id = {contactId}");

            CustomerTimeChimp customer = customerHelperTC.GetCustomers().Find(c => c.RelationId != null && c.RelationId.Equals(ETSContact.CO_KLCOD)) ?? throw new Exception($"Customer with number = {ETSContact.CO_KLCOD} doesn't exist in TimeChimp");
            int customerId = customer.Id;

            //change to timechimp class
            ContactTimeChimp TCContact = new(ETSContact, customerId);

            //check if contact exists in timechimp
            return contactHelperTC.ContactExists(ETSContact)
                ? Results.Ok(contactHelperTC.UpdateContact(TCContact))
                : Results.Ok(contactHelperTC.CreateContact(TCContact));
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
            return Results.Ok(uurcodeHelperETS.GetUurcodes(DateTime.Parse(dateString)));
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
            UurcodeETS? ETSUurcode = uurcodeHelperETS.GetUurcode(uurcodeId);

            // Handle when uurcode doesn't exist in ETS
            if (ETSUurcode == null)
            {
                return Results.Problem($"ETS doesn't contain an uurcode with id = {uurcodeId}");
            }

            //change to timechimp class
            UurcodeTimeChimp TCUurcode = new(ETSUurcode);

            //check if uurcode exists in timechimp
            return uurcodeHelperTC.UurcodeExists(uurcodeId)
                ? Results.Ok(uurcodeHelperTC.UpdateUurcode(TCUurcode))
                : Results.Ok(uurcodeHelperTC.CreateUurcode(TCUurcode));
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
            return Results.Ok(employeeHelperETS.GetEmployeeIdsChangedAfter(DateTime.Parse(dateString), teamName));
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
            EmployeeETS? ETSEmployee = employeeHelperETS.GetEmployee(employeeId);

            // Handle when contact doesn't exist in ETS
            if (ETSEmployee == null)
            {
                return Results.Problem($"ETS doesn't contain an employee with id = {employeeId}");
            }

            //determine role by most used role for all users except admins/managers
            int roleId = employeeHelperTC.GetEmployees()
               .Where(e => e.Role.Id > 4 || e.Role.Id == 1)
               .GroupBy(e => e.Role.Id)
               .Select(g => new
               {
                   RoleId = g.Key,
                   Count = g.Count()
               })
               .MaxBy(o => o.Count)?.RoleId ?? 1;
            roleId = 4;

            //change to timechimp class
            EmployeeTimeChimp TCEmployee = new(ETSEmployee, roleId);

            //check if employee exists in timechimp
            if (employeeHelperTC.EmployeeExists(employeeId))
            {
                projectUserHelperTC.AddAllProjectUserForEmployee(int.Parse(employeeId));
                return Results.Ok(employeeHelperTC.UpdateEmployee(TCEmployee));
            }
            else
            {
                //check if employee has an emailaddress
                if (TCEmployee.UserName == null)
                {
                    return Results.Problem($"Can't create the employee {TCEmployee.DisplayName} without an emailaddress");
                }

                EmployeeTimeChimp employee = employeeHelperTC.CreateEmployee(TCEmployee);
                TCEmployee.Id = employee.Id;
                employee = employeeHelperTC.UpdateEmployee(TCEmployee);

                //adds employee to all existing projects in TimeChimp
                projectUserHelperTC.AddAllProjectUserForEmployee(employee.Id);

                return Results.Ok(employee);
            }
        }
        catch (Exception exception)
        {
            return Results.Problem(exception.Message, exception.Data.ToString());
        }

    }).WithName($"{company}SyncEmployeeTimechimp").WithTags(company);

    //get projectids from ets
    app.MapGet($"/api/{company.ToLower()}/ets/projectids", (string dateString) =>
    {
        try
        {
            return Results.Ok(projectHelperETS.GetProjectIdsChangedAfter(DateTime.Parse(dateString)));
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
            // Get project from ETS
            ProjectETS ETSProject = projectHelperETS.GetProject(projectId);

            // Handle when project doesn't exist in ETS
            if (ETSProject == null)
            {
                return Results.Problem($"ETS doesn't contain a project with id = {projectId}");
            }

            // Change to TimeChimp class
            ProjectTimeChimp TCProject = new(ETSProject);
            EmployeeTimeChimp ManagerId = employeeHelperTC.GetEmployeeByEmployeeNumber(ETSProject.PR_BESTEMMELING ?? null);

            CustomerTimeChimp customer;
            // Find customer id TimeChimpETSContact.CO_KLCOD
            if (ETSProject.PR_KLNR == null)
            {
                customer = customerHelperTC.GetCustomers().Find(c => c.Intern != null && c.Intern.Value) ?? throw new Exception($"The ETS record for project with id = {projectId} has no customernumber, internal customer is still archived in TimeChimp!");
            }
            else
            {
                customer = customerHelperTC.GetCustomers().Find(c => c.RelationId != null && c.RelationId.Equals(ETSProject.PR_KLNR)) ?? throw new Exception($"No timechimp cutomer found with id = {ETSProject.PR_KLNR}");
            }
            TCProject.Customer = new()
            {
                Id = customer.Id
            };

            ProjectTimeChimp mainProject = projectHelperTC.FindProject(projectId) ?? projectHelperTC.CreateProject(TCProject);

            // update mainproject
            TCProject.Id = mainProject.Id;

            mainProject = projectHelperTC.UpdateProject(TCProject);

            List<string> errorMessages = new();
            double totalBudgetHours = 0;
            List<UurcodeTimeChimp> uurcodes = uurcodeHelperTC.GetUurcodes();
            // get subprojects from ETS
            List<SubprojectETS> ETSSubprojects = projectHelperETS.GetSubprojects(projectId);
            foreach (SubprojectETS ETSSubproject in ETSSubprojects.Where(subProject => subProject.SU_SUB != null && !subProject.SU_SUB.StartsWith('2'))) //  only iterate subprojects with ids from [0000, 2000[ en [3000, ...]
            {
                double budgetHours = 0;
                // Change to TimeChimp class
                ProjectTimeChimp TCSubproject = new(ETSSubproject, mainProject);

                ProjectTimeChimp subProject = projectHelperTC.FindProject(TCSubproject.Code ?? throw new Exception("Subproject received from timechimp has no code")) ?? projectHelperTC.CreateProject(TCSubproject);

                TCSubproject.Id = subProject.Id;
                subProject = projectHelperTC.UpdateProject(TCSubproject);


                subProject.ProjectTasks.Clear();
                subProject.ProjectUsers.Clear();

                if (TCSubproject.Active ?? false)
                {
                    //update budgethours for each projecttask in timeChimp
                    List<ProjectTaskETS> projectTasksETS = uurcodeHelperETS.GetUurcodesSubproject(ETSProject.PR_NR ?? throw new Exception("Project received from ETS has no NR"), ETSSubproject.SU_SUB ?? throw new Exception("Subproject received from ETS has no SUBNR"));
                    foreach (ProjectTaskETS projectTaskETS in projectTasksETS)
                    {
                        if (float.Parse(projectTaskETS.VO_AANT.ToString()) == 0)
                        {
                            break;
                        }
                        if (subProject.Id == null)
                        {
                            errorMessages.Add($"Subproject {subProject.Code} has no id");
                        }
                        else if (string.IsNullOrEmpty(projectTaskETS.VO_UUR?.Trim()))
                        {
                            errorMessages.Add($"Subproject {subProject.Code} field VO_UUR is empty in table J2W_VOPX");
                        }
                        else if (projectTaskETS.VO_AANT == null)
                        {
                            errorMessages.Add($"Subproject {subProject.Code} field VO_AANT is null in table J2W_VOPX");
                        }
                        else
                        {
                            int taskId = uurcodes.Find(u => u.Code != null && u.Code.Equals(projectTaskETS.VO_UUR))?.Id ?? throw new Exception($"TimeChimp has no task with code = {projectTaskETS.VO_UUR}");
                            ProjectTaskTC projectTaskTC = new()
                            {
                                Task = new TaskTC
                                {
                                    Id = taskId
                                },
                                BudgetHours = float.Parse(projectTaskETS.VO_AANT.Value.ToString())
                            };

                            subProject.ProjectTasks.Add(projectTaskTC);

                            totalBudgetHours += projectTaskETS.VO_AANT.Value;
                            budgetHours += projectTaskETS.VO_AANT.Value;
                        }
                    }
                }

                List<int> userIds = employeeHelperTC.GetEmployees().Where(e => e.Active == true).Select(e => e.Id).ToList();

                foreach (int userId in userIds)
                {
                    subProject.ProjectUsers ??= new List<ProjectUserTC>();

                    subProject.ProjectUsers.Add(new ProjectUserTC
                    {
                        User = new UserTC
                        {
                            Id = userId
                        }
                    });
                }
                subProject.Budget.Hours = float.Parse(budgetHours.ToString());
                subProject.Budget.Method = "TaskHours";
                subProject.Managers.Clear();
                if (ManagerId != null)
                {
                    subProject.Managers.Add(new Manager
                    {
                        Id = ManagerId.Id
                    });
                }

                subProject = projectHelperTC.UpdateProject(subProject);
            }

            // update mainproject
            TCProject.Id = mainProject.Id;
            TCProject.Budget.Hours = float.Parse(totalBudgetHours.ToString());
            List<int> usersIds = employeeHelperTC.GetEmployees().Where(e => e.Active == true).Select(e => e.Id).ToList();
            if (TCProject.ProjectUsers == null)
            {
                TCProject.ProjectUsers = new List<ProjectUserTC>();
            }
            TCProject.ProjectUsers.Clear();
            foreach (int userId in usersIds)
            {
                TCProject.ProjectUsers ??= new List<ProjectUserTC>();

                TCProject.ProjectUsers.Add(new ProjectUserTC
                {
                    User = new UserTC
                    {
                        Id = userId
                    }
                });
            }

            TCProject.Managers.Clear();
            if (ManagerId != null)
            {
                TCProject.Managers.Add(new Manager
                {
                    Id = ManagerId.Id
                });
            }
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
    app.MapGet($"/api/{company.ToLower()}/ets/timeids", () =>
    {
        try
        {
            return Results.Ok(timeHelperTC.GetTimes());
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
    app.MapGet($"/api/{company.ToLower()}/ets/mileageids", () =>
    {
        try
        {
            return Results.Ok(mileageHelperTC.GetApprovedMileageIds());
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
            MileageTimeChimp mileage = mileageHelperTC.GetMileage(mileageId);

            //mileage.Vehicle = vehicleHelper.GetVehicle(mileage.Vehicle.Id) ?? throw new Exception("Vehicle received from timechimp has no vehicle");
            Console.WriteLine(JsonTool.ConvertFromWithNullValues(mileage));

            string projectNumber = projectHelperTC.GetProject(mileage.Project.Id).Code ?? throw new Exception("Project received from timechimp has no code");
            string employeeNumber = employeeHelperTC.GetEmployee(mileage.User.Id).EmployeeNumber ?? throw new Exception("Employee received from timechimp has no employeeNumber");

            MileageETS mileageETS = new(mileage, projectNumber, employeeNumber);
            MileageETS response = mileageHelperETS.UpdateMileage(mileageETS);

            //change status
            MileageTimeChimp responseStatus = mileageHelperTC.changeStatus(mileageId);

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
            List<PurchaseOrderHeaderETS> purchaseOrders = purchaseOrderHelperETS.GetOpenPurchaseOrders();
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
            PurchaseOrderHeaderETS header = purchaseOrderHelperETS.GetPurchaseOrderHeader(id);


            List<PurchaseOrderDetailETS> purchaseOrders = purchaseOrderHelperETS.GetPurchaseOrderDetails(id);

            //convert to a dictionary for the web
            Dictionary<string, object?> result = new()
                {
                    {"bonNummer", id},
                    {"artikels", new List<Dictionary<string, object?>>()},
                    {"klant", header.KL_NAM},
                    {"project", header.FH_PROJ},
                    {"subproject", header.FH_SUBPROJ}
                };
            foreach (PurchaseOrderDetailETS purchaseOrder in purchaseOrders.Where(p => p.FD_ARTNR != null))
            {
                ((List<Dictionary<string, object?>>)(result["artikels"] ?? throw new Exception("Value at key artikels is null"))).Add(new()
                    {
                        {"artikelNummer", articleHelperETS.GetArticleReference(purchaseOrder.FD_ARTNR ?? throw new Exception($"PurchaseOrder ETS with number = {purchaseOrder.FD_BONNR} has no ARTNR"), header.FH_KLNR ?? throw new Exception($"PurchaseOrder ETS with number = {purchaseOrder.FD_BONNR} has no KLNR"))},
                        {"omschrijving", purchaseOrder.ART_OMS},
                        {"aantal", purchaseOrder.TOTAAL_AANTAL},
                        {"leverancier", header.LV_NAM}
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
    app.MapGet($"/api/{company.ToLower()}/ets/createpurchasefile", (string id, string seperator, bool forceCSV) =>
    {
        try
        {
            PurchaseOrderHeaderETS header = purchaseOrderHelperETS.GetPurchaseOrderHeader(id);

            string supplier = header.LV_NAM ?? throw new Exception($"PurchaseOrder header in ETS with number = {header.FH_BONNR} has no LV_NAM");
            string supplierId = header.FH_KLNR ?? throw new Exception($"PurchaseOrder header in ETS with number = {header.FH_BONNR} has no FH_KLNR");

            List<PurchaseOrderDetailETS> purchaseOrders = purchaseOrderHelperETS.GetPurchaseOrderDetails(id);
            purchaseOrders.ForEach(po => po.FD_KLANTREFERENTIE = articleHelperETS.GetArticleReference(po.FD_ARTNR ?? throw new Exception($"PurchaseOrder detail in ETS with number = {po.FD_BONNR} has no ART_NR"), supplierId));
            purchaseOrders = purchaseOrders.Where(po => po.FD_KLANTREFERENTIE != null).ToList();

            FileContentResult fileContent;
            if (!forceCSV && supplier.ToLower().Contains("cebeo"))
            {
                List<Dictionary<string, object>> orderLines = new();
                purchaseOrders.ForEach(purchaseOrder =>
                {
                    if (purchaseOrder.FD_KLANTREFERENTIE != null)
                    {
                        string cebeoArticleNumber = articleHelperCebeo.SearchForArticleWithReference(purchaseOrder.FD_KLANTREFERENTIE)?.Material?.SupplierItemID ?? throw new Exception($"Cebeo has no article with reference = {purchaseOrder.FD_KLANTREFERENTIE}");
                        orderLines.Add(new Dictionary<string, object>()
                        {
                            { "number", cebeoArticleNumber },
                            { "aantal", purchaseOrder.TOTAAL_AANTAL ?? 0 }
                        });
                    }
                });

                fileContent = purchaseOrderHelperETS.CreateFileCebeo(orderLines, id, supplier, config);
            }
            else
            {
                fileContent = purchaseOrderHelperETS.CreateCSVFile(purchaseOrders, supplier, seperator);
            }

            return Results.Ok(fileContent);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}CreatePurchaseFile").WithTags(company);

    //get all articles from cebeo
    app.MapGet($"/api/{company.ToLower()}/cebeo/articles", () =>
    {
        try
        {
            string supplierId = supplierHelperETS.FindSupplierId("cebeo");
            List<string> articles = articleHelperETS.GetAricles(supplierId);

            return Results.Ok(articles);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}GetArticles").WithTags(company);

    //update article from cebeo in ets
    app.MapGet($"/api/{company.ToLower()}/cebeo/updatearticleprice", (string articleNumberETS, float maxPriceDiff) =>
    {
        try
        {
            string articleReference = (articleHelperETS.GetArticle(articleNumberETS) ?? throw new Exception($"ETS han no article with number = {articleNumberETS}")).ART_LEVREF ?? throw new Exception($"Article in ETS with number = {articleNumberETS}, has no supplier reference number");

            float newPrice = articleHelperCebeo.GetArticlePriceCebeo(articleReference);

            ArticleETS article = articleHelperETS.UpdateArticlePriceETS(articleNumberETS, newPrice, maxPriceDiff);

            float updatedPrice = article.ART_AANKP ?? throw new Exception($"Article from ETS with number = {articleNumberETS} has no AANKP");
            return Results.Ok(updatedPrice == newPrice ? $"Price updated to {updatedPrice}" : $"Price not updated, price diff is {Math.Abs(updatedPrice - newPrice) / article.ART_AANKP * 100}%");
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}UpdateArticlePrice").WithTags(company);

    //searches for article in cebeo
    app.MapGet($"/api/{company.ToLower()}/cebeo/searcharticle", (string articleReference) =>
    {
        try
        {
            string supplierId = supplierHelperETS.FindSupplierId("cebeo");

            if (articleHelperETS.ArticleWithReferenceExists(articleReference, supplierId))
            {
                throw new Exception($"ETS already has an article with reference = {articleReference}");
            }

            if (articleHelperETS.ArticleWithNumberExists(articleReference))
            {
                throw new Exception($"ETS already has an article with number = {articleReference}");
            }

            CebeoItem articleCebeo = articleHelperCebeo.SearchForArticleWithReference(articleReference) ?? throw new Exception($"Cebeo has no article with reference = {articleReference}");

            ArticleWeb article = new(articleCebeo);

            return Results.Ok(article);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}SearchArticleCebeo").WithTags(company);

    //get article form info
    app.MapGet($"/api/{company.ToLower()}/ets/articleforminfo", () =>
    {
        try
        {
            string query = "SELECT AR_COD AS CODE, AR_OMS1 AS DESCRIPTION FROM ARTFAM";
            string responseFamilies = ETSClient.selectQuery(query);

            query = "SELECT ASF_COD AS CODE, ASF_OMS1 AS DESCRIPTION FROM ARTSUBFAM";
            string responseSubfamilies = ETSClient.selectQuery(query);

            query = "SELECT EH_COD AS CODE, EH_OMS1 AS DESCRIPTION, EH_OMS2 AS SHORT_DESCRIPTION FROM EENHEID";
            string responseMeasureTypes = ETSClient.selectQuery(query);

            query = "SELECT TBL_REKENING.REK_CODE AS CODE, TBL_REKENING_TAAL.RET_OMSCHRIJVING AS DESCRIPTION FROM TBL_REKENING LEFT JOIN TBL_REKENING_TAAL ON TBL_REKENING.REK_ID = TBL_REKENING_TAAL.RET_MASTER_ID";
            string responseBankAccounts = ETSClient.selectQuery(query);

            query = "SELECT EURO_COD AS CODE, EURO_OMS AS DESCRIPTION FROM EURO";
            string responseCoinTypes = ETSClient.selectQuery(query);

            query = "SELECT CODE, OMSCHRIJF AS DESCRIPTION FROM BTW_CODE";
            string responseBTWCodes = ETSClient.selectQuery(query);

            List<string> suppliers = config.GetSection($"Operations:{company}").GetChildren().Select(x => x.Value.ToUpper()).Distinct().ToList();
            suppliers.Add("CEBEO".ToUpper());
            string queryPart = string.Join("' OR UPPER(LV_NAM) LIKE '", suppliers.Select(s => $"%{s}%").ToList());

            query = "SELECT LV_COD AS CODE, LV_NAM AS NAME FROM LVPX WHERE (UPPER(LV_NAM) LIKE '" + queryPart + "') AND NOT UPPER(LV_NAM) LIKE '%BV%'";
            string responseSuppliers = ETSClient.selectQuery(query);

            Dictionary<string, object> result = new()
            {
                {"families", JsonTool.ConvertTo<List<Dictionary<string, object>>>(responseFamilies)},
                {"subfamilies", JsonTool.ConvertTo<List<Dictionary<string, object>>>(responseSubfamilies)},
                {"measureTypes", JsonTool.ConvertTo<List<Dictionary<string, object>>>(responseMeasureTypes)},
                {"bankAccounts", JsonTool.ConvertTo<List<Dictionary<string, object>>>(responseBankAccounts)},
                {"coinTypes", JsonTool.ConvertTo<List<Dictionary<string, object>>>(responseCoinTypes)},
                {"BTWCodes", JsonTool.ConvertTo<List<Dictionary<string, object>>>(responseBTWCodes)},
                {"suppliers", JsonTool.ConvertTo<List<Dictionary<string, object>>>(responseSuppliers)}
            };

            return Results.Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}ArticleFormInfo").WithTags(company);

    //validate article form
    app.MapPost($"/api/{company.ToLower()}/ets/validatearticleform", ([FromBody] ArticleWeb article) =>
    {
        try
        {
            Dictionary<string, string[]> problems = new();
            articleHelperETS.ValidateArticle(article).ToList().ForEach(x => problems.Add(x.Key, x.Value.ToArray()));

            if (problems.Any(x => x.Value.Length > 0))
            {
                return Results.ValidationProblem(problems);
            }

            return Results.Ok(article);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}ValidateArticleForm").WithTags(company);

    //create article in ets
    app.MapPost($"/api/{company.ToLower()}/ets/createarticle", ([FromBody] ArticleWeb article) =>
    {
        try
        {
            return Results.Ok(articleHelperETS.CreateArticle(article));
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}CreateArticle").WithTags(company);

    //transform bom excel file to json
    app.MapPost($"/api/{company.ToLower()}/ets/transformbomexcel", ([FromBody] OwnFileContentResult excelFile, string fileName) =>
    {
        try
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using var stream = new MemoryStream(excelFile.FileContents ?? throw new Exception("File given has no content"));
            using var reader = ExcelReaderFactory.CreateReader(stream);
            DataSet data = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });

            Item MainPart = new Item();
            string number = fileName.Split('.')[0].Split('_')[0];
            string omschrijving = fileName.Split('.')[0].Split('_')[1];
            MainPart.Number = number;
            MainPart.Description = omschrijving;
            MainPart.LynNumber = "1";
            MainPart.Bewerking1 = "Monteren";

            List<Item> MetabilItems = new List<Item>();

            DataTable table = data.Tables["BOM"] ?? throw new Exception("There was not table found in excel with the name (BOM)");

            List<Item?> assemblies = new();
            foreach (DataRow row in table.Rows)
            {
                List<int> hierarchy = row["Item"]?.ToString()?.Split('.')?.Select(x => int.Parse(x) - 1)?.ToList() ?? new();
                List<Item?> parentList = assemblies;
                foreach (int level in hierarchy)
                {
                    while (level >= parentList.Count)
                    {
                        parentList.Add(null);
                    }

                    if (parentList[level] != null)
                    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        parentList = parentList[level].Parts;
                    }
                    else
                    {
#pragma warning disable CS8604 // Dereference of a possibly linenull reference.
                        Item part = new(row["Part Number"]?.ToString().Split("_").First(), row["Description"] is System.DBNull ? String.Empty : (string)row["Description"].ToString().ToUpper(), Convert.ToInt32((double)row["QTY"]), row["Item"]?.ToString()?.Split('.')?.ToList().Last());
#pragma warning restore CS8604 // Dereference of a possibly null reference.
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                        //if (part.Number is not null && part.Number.Length > 25)
                        //{
                        //    throw new Exception($"Excel contains an item where the part number is longer then 25 characters \"{part.Number}\", this is not allowed");
                        //}
                        try
                        {

                            part.MainSupplier = row["VENDOR"] != DBNull.Value ? ((string)row["VENDOR"]) : string.Empty;

                            part.Bewerking1 = row["Bewerking 1"] != DBNull.Value ? ((string)row["Bewerking 1"]).ToUpper().Trim() : "-";
                            part.Bewerking2 = row["Bewerking 2"] != DBNull.Value ? ((string)row["Bewerking 2"]).ToUpper().Trim() : "-";
                            part.Bewerking3 = row["Bewerking 3"] != DBNull.Value ? ((string)row["Bewerking 3"]).ToUpper().Trim() : "-";
                            part.Bewerking4 = row["Bewerking 4"] != DBNull.Value ? ((string)row["Bewerking 4"]).ToUpper().Trim() : "-";
                            part.Nabehandeling1 = row["Nabehandeling 1"] != DBNull.Value ? ((string)row["Nabehandeling 1"]).ToUpper().Trim() : "-";
                            part.Nabehandeling2 = row["Nabehandeling 2"] != DBNull.Value ? ((string)row["Nabehandeling 2"]).ToUpper().Trim() : "-";

                            part.Mass = row["Mass"] != DBNull.Value && float.TryParse(row["Mass"].ToString().Split(' ')[0], out float massValue) ? float.Parse(row["Mass"].ToString().Split(' ')[0]) : 0;

                            part.Aankoopeenh = row["Aankoopeenh"] != DBNull.Value ? ((string)row["Aankoopeenh"]).ToUpper().Trim() : "ST";
                            part.AankoopPer = row["Aankoop per"] != DBNull.Value ? (row["Aankoop per"].ToString()) : "1";
                            part.Verbruikseenh = row["Verbruikseenh"] != DBNull.Value ? ((string)row["Verbruikseenh"]).ToUpper().Trim() : "ST";
                            part.Omrekeningsfactor = row["Omrekeningsfactor"] != DBNull.Value ? ((string)row["Omrekeningsfactor"].ToString()) : "1";
                            part.TypeFactor = row["Type Factor"] != DBNull.Value ? ((string)row["Type Factor"]).Trim() : "Deelfactor";


                            parentList[level] = part;
                            bool save = true;

                            void inList(Item item)
                            {
                                if (item.Parts.Count > 0)
                                {
                                    foreach (Item? item2 in item.Parts)
                                    {
                                        inList(item2);
                                    }
                                }

                                if (part.Number == item.Number)
                                {
                                    save = false;
                                }
                            }

                            if (part.Number.EndsWith('W'))
                            {
                                foreach (Item item in MetabilItems)
                                {
                                    inList(item);
                                }
                                if (save)
                                {
                                    MetabilItems.Add(part);
                                }
                            }

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            return Results.Problem(e.Message);
                        }
                    }
                }
            }
            MainPart.Parts = assemblies;
            List<Item> Main = new()
            {
                MainPart
            };

            List<List<Item>> items = new()
            {
                Main,
                MetabilItems
            };

            return Results.Ok(items);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}TransformBomExcel").WithTags(company);

    //update article links in ets
    app.MapPut($"/api/{company.ToLower()}/ets/updatelinkedarticles", ([FromBody] List<Item> articles) =>
    {
        try
        {
            List<Dictionary<string, string>> logs = new List<Dictionary<string, string>>();
            if (articles == null || !articles.Any())
            {
                // Handle the case where no articles are provided
                return Results.BadRequest("No articles provided.");
            }

            void LinkArticles(Item main)
            {
                if (main.Parts != null)
                {
                    foreach (Item part in main.Parts.Where(i => i != null).OfType<Item>())
                    {
                        articleHelperETS.LinkArticle(main, part);
                        Dictionary<string, string> log = new()
                    {
                        {"artikelNumber", part.Number },
                        {"action", "link" },
                        {"extra", "linked to "+main.Number }
                    };
                        logs.Add(log);
                        LinkArticles(part);
                    }
                }
            }

            foreach (Item article in articles)
            {
                LinkArticles(article);
            }

            return Results.Ok(logs);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}UpdateLinkedArticles").WithTags(company);

    //get projecten voortgang from ets
    app.MapGet($"/api/{company.ToLower()}/ets/projectenvoortgang", () =>
    {

        return projVoortgangHelperETS.GetProjectenVoortgang();
    }).WithName($"{company}ProjectenVoortgang").WithTags(company);

    //get an boolean if article exists in ets
    app.MapGet($"/api/{company.ToLower()}/ets/articleExists", (string ArticleNumber) =>
    {
        return Results.Ok(articleHelperETS.ArticleWithNumberExists(ArticleNumber));
    }).WithName($"{company}ArticleExists").WithTags(company);

    //gets differences between articles in ets and bom
    app.MapPost($"/api/{company.ToLower()}/ets/articledifference", ([FromBody] Item articles) =>
    {
        Dictionary<string, Change> difference = articleHelperETS.ArticleDifference(articles);
        return difference;

    }).WithName($"{company}ArticleDifference").WithTags(company);

    //updates article in ets
    app.MapPut($"/api/{company.ToLower()}/ets/updateitem", ([FromBody] ItemChange item) =>
    {
        try
        {
            Dictionary<string, string> log = itemHelperETS.UpdateItem(item);

            return Results.Ok(log);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}Updateitem").WithTags(company);

    //create article in ets
    app.MapPost($"/api/{company.ToLower()}/ets/createitem", ([FromBody] NewItem item) =>
    {
        try
        {
            if (item.Hoofdleverancier.ToLower() == "lassen" || item.Hoofdleverancier.ToLower() == "oplassen" && company.ToLower() == "metabil")
            {
                item.Hoofdleverancier = "";
            }
            else
            {
                item.Hoofdleverancier = bewerkingenSettings[company.ToUpper()][item.Hoofdleverancier];
            }

            Dictionary<string, string> log = itemHelperETS.CreateItem(item);

            return Results.Ok(log);
        }
        catch (Exception e)
        {
            Console.WriteLine($"error: {e.Message}");
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}CreateItem").WithTags(company);

    //sync timechimp via python file
    app.MapGet($"/api/{company.ToLower()}/sync", () =>
    {
        try
        {
            string argument = config["SyncFilePath"] + " " + company.ToLower();
            ProcessStartInfo start = new()
            {
                FileName = "python",
                Arguments = $"R:\\Devion Software\\Scripts\\algemene-sync.py {company.ToLower()}",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };
            using Process process = Process.Start(start);
            using StreamReader reader = process.StandardOutput;
            string result = reader.ReadToEnd();
            return Results.Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine($"error: {e.Message}");
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}Sync").WithTags(company);

    app.MapGet($"/api/{company.ToLower()}/sync/start", () =>
    {
        if (company.ToLower() == "metabil")
        {
            syncMetabil = 1;
        }
        else if (company.ToLower() == "devion")
        {
            syncDevion = 1;
        }
        return Results.Ok("Sync Started");
    }).WithName($"{company}SyncStart").WithTags(company);

    app.MapGet($"api/{company.ToLower()}/sync/stop", () =>
    {
        if (company.ToLower() == "metabil")
        {
            syncMetabil = 0;
        }
        else if (company.ToLower() == "devion")
        {
            syncDevion = 0;
        }
        return Results.Ok("Sync Stopped");
    }).WithName($"{company}SyncStop").WithTags(company);

    app.MapGet($"/api/{company.ToLower()}/sync/status", () =>
    {
        Dictionary<string, int> sync = new();
        if (company.ToLower() == "metabil")
        {
            sync.Add("sync", syncMetabil);
        }
        else if (company.ToLower() == "devion")
        {
            sync.Add("sync", syncDevion);
        }
        return Results.Ok(sync);
    }).WithName($"{company}SyncStatus").WithTags(company);

    app.MapGet($"api/{company.ToLower()}/projecten/voortgang/export", () =>
    {
        Console.WriteLine("Exporting projecten voortgang");
        return Results.Ok();
    }).WithName($"{company}ProjectenVoortgangExport").WithTags(company);

    app.MapPost($"/api/{company.ToLower()}/projecten/voortgang/import", ([FromBody] OwnFileContentResult excelFile, string FileName) =>
    {
        try
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using var stream = new MemoryStream(excelFile.FileContents ?? throw new Exception("File given has no content"));
            using var reader = ExcelReaderFactory.CreateReader(stream);
            DataSet data = reader.AsDataSet(new ExcelDataSetConfiguration()
            {
                ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                {
                    UseHeaderRow = true
                }
            });

            DataTable tableTask = data.Tables["Task_Table"] ?? throw new Exception("There wasn't a table fond with the name (Task_Table)");
            DataTable tableResource = data.Tables["Resource_Table"] ?? throw new Exception("There wasn't a table fond with the name (Resource_Table)");
            DataTable tableAssignment = data.Tables["Assignment_Table"] ?? throw new Exception("There wasn't a table fond with the name (Assignment_Table)");

            List<ProjectVoortgangTask> tasks = new List<ProjectVoortgangTask>();

            if (tableTask.Rows.Count != 0)
            {
                foreach (DataRow row in tableTask.Rows)
                {
                    ProjectVoortgangTask task = new()
                    {
                        Id = Int32.Parse((string)row["ID"]),
                        Active = row["Active"] == DBNull.Value ? "" : (string)row["Active"],
                        TaskMode = row["Task Mode"] == DBNull.Value ? "" : (string)row["Task Mode"],
                        Name = row["Name"] == DBNull.Value ? "" : (string)row["Name"],
                        Duration = row["Duration"] == DBNull.Value ? "" : (string)row["Duration"],
                        Start = row["Start"] == DBNull.Value ? "" : (string)row["Start"],
                        Finish = row["Finish"] == DBNull.Value ? "" : (string)row["Finish"],
                        Predecessors = row["Predecessors"] == DBNull.Value ? "" : (string)row["Predecessors"],
                        OutlineLevel = Int32.Parse((string)row["Outline Level"]),
                        Notes = row["Notes"] == DBNull.Value ? "" : (string)row["Notes"],
                    };
                    task.Name = task.Name.Split(' ').First();
                    task.Duration = task.Duration.Split(' ').First();
                    tasks.Add(task);
                }
            }

            foreach (ProjectVoortgangTask task in tasks)
            {
                if (task.Predecessors != "")
                {
                    string[] predecessors = task.Predecessors.Split(',');
                    string newPredecessors = "";
                    foreach (string predecessor in predecessors)
                    {
                        string[] splitPredecessor = predecessor.Split(' ');
                        if (newPredecessors != "")
                        {
                            newPredecessors += ", " + tasks.Find(t => t.Id == Int32.Parse(splitPredecessor[0])).Name;
                        }
                        else
                        {
                            newPredecessors += tasks.Find(t => t.Id == Int32.Parse(splitPredecessor[0])).Name;
                        }
                    }
                    task.Predecessors = newPredecessors;
                }
                Console.WriteLine(task.Predecessors);
            }
            return Results.Ok(tasks);
        }
        catch (Exception e)
        {
            Console.WriteLine($"error: {e.Message}");
            Console.WriteLine($"line: {e.StackTrace}");
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}ProjectenVoortgangImport").WithTags(company);

    app.MapGet($"/api/{company.ToLower()}/foldernames", () =>
    {
        string baseFolderpath = @"C:\Users\mathias\Devion\MechanicalDesign - Mechanical Projecten";

        List<string> folders = new();

        foreach (string folderName in Directory.EnumerateDirectories(baseFolderpath))
        {
            folders.Add(Path.GetFileName(folderName));
        }

        return Results.Ok(folders);
    }).WithName($"{company}FolderNames").WithTags(company);

    app.MapPost($"/api/{company.ToLower()}/tekeningen/check", ([FromBody] Item MainPart, string project) =>
    {
        try
        {
            Console.WriteLine(MainPart.Number);
            Console.WriteLine(project);
            string baseFolderpath = @"C:\Users\mathias\Devion\MechanicalDesign - Mechanical Projecten\" + project + @"\11_Productie\05_PDF_DXF_STP_Compleet\";
            MainPart = itemHelperETS.CheckFiles(MainPart, baseFolderpath);
            return Results.Ok(MainPart);
        }
        catch (Exception e)
        {
            Console.WriteLine($"error: {e.Message}");
            Console.WriteLine($"line: {e.StackTrace}");
            return Results.Problem(e.Message);
        }

    }).WithName($"{company}CheckTekenigen").WithTags(company);
}

app.MapGet("/api/companies", () => Results.Ok(companies)).WithName($"GetCompanyNames");

app.MapGet("/api/price", () =>
{
    try
    {
        Dictionary<string, Dictionary<string, float>> data = new Dictionary<string, Dictionary<string, float>>();
        string json = File.ReadAllText("./Json/priceSettings.json");
        data = JsonTool.ConvertTo<Dictionary<string, Dictionary<string, float>>>(json);
        priceSettings = data;
        return Results.Ok(data);
    }
    catch (Exception e)
    {
        Console.WriteLine($"error: {e.Message}");
        return Results.Problem(e.Message);
    }
}).WithName($"GetPriceSettings");

app.MapPost("/api/price", ([FromBody] Dictionary<string, Dictionary<string, float>> settings) =>
{
    try
    {
        //write to file
        priceSettings = settings;
        string json = JsonTool.ConvertFrom(settings);
        File.WriteAllText("./Json/priceSettings.json", json);
        return Results.Ok(settings);
    }
    catch (Exception e)
    {
        Console.WriteLine($"error: {e.Message}");
        return Results.Problem("Error while writing to file");
    }
}).WithName($"PostPriceSettings");

app.MapGet("/api/bewerkingen", () =>
{
    try
    {
        Dictionary<string, Dictionary<string, string>> data = new Dictionary<string, Dictionary<string, string>>();
        string json = File.ReadAllText("./Json/bewerkingenSettings.json");
        data = JsonTool.ConvertTo<Dictionary<string, Dictionary<string, string>>>(json);
        bewerkingenSettings = data;
        return Results.Ok(data);
    }
    catch (Exception e)
    {
        Console.WriteLine($"error: {e.Message}");
        return Results.Problem(e.Message);
    }
}).WithName($"GetBewerkingenSettings");

app.MapPost("/api/bewerkingen", ([FromBody] Dictionary<string, Dictionary<string, string>> settings) =>
{
    try
    {
        //write to file
        bewerkingenSettings = settings;
        string json = JsonTool.ConvertFrom(settings);
        File.WriteAllText("./Json/bewerkingenSettings.json", json);
        return Results.Ok(settings);
    }
    catch (Exception e)
    {
        Console.WriteLine($"error: {e.Message}");
        return Results.Problem("Error while writing to file");
    }
}).WithName($"PostBewerkingenSettings");

if (app.Environment.IsProduction())
{
    app.Run("http://*:5000");
}
else
{
    app.Run();
}

