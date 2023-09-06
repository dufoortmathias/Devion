WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ConfigurationManager config = builder.Configuration;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigins", builder => builder.WithOrigins(config["AllowedHosts"]));
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowOrigins");
//app.UseHttpsRedirection();



List<string> companies = new();

int companyIndex = -1;
while (config[$"Companies:{++companyIndex}:Name"] != null)
{
    WebClient TCClient = new(config["TimeChimpBaseURL"], config[$"Companies:{companyIndex}:TimeChimpToken"]);
    FirebirdClientETS ETSClient = new(config["ETSServer"], config[$"Companies:{companyIndex}:ETSUser"], config[$"Companies:{companyIndex}:ETSPassword"], config[$"Companies:{companyIndex}:ETSDatabase"]);

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

    TimeChimpContactHelper contactHelperTC = new(TCClient);
    TimeChimpCustomerHelper customerHelperTC = new(TCClient);
    TimeChimpEmployeeHelper employeeHelperTC = new(TCClient);
    TimeChimpMileageHelper mileageHelperTC = new(TCClient);
    TimeChimpProjectHelper projectHelperTC = new(TCClient);
    TimeChimpProjectUserHelper projectUserHelperTC = new(TCClient);
    TimeChimpTimeHelper timeHelperTC = new(TCClient, ETSClient);
    TimeChimpUurcodeHelper uurcodeHelperTC = new(TCClient);

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

            CustomerTimeChimp customer = customerHelperTC.GetCustomers().Find(c => c.relationId != null && c.relationId.Equals(ETSContact.CO_KLCOD)) ?? throw new Exception($"Customer with number = {ETSContact.CO_KLCOD} doesn't exist in TimeChimp");
            int customerId = customer.id ?? throw new Exception("Customer received from timechimp has no id");

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
            return uurcodeHelperTC.uurcodeExists(uurcodeId)
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
                .Where(e => e.roleId > 4 || e.roleId == 1)
                .GroupBy(e => e.roleId ?? throw new Exception("Employee received from timechimp has no roleId"))
                .Select(g => new
                {
                    RoleId = g.Key,
                    Count = g.Count()
                })
                .MaxBy(o => o.Count)?.RoleId ?? 1;

            //change to timechimp class
            EmployeeTimeChimp TCEmployee = new(ETSEmployee, roleId);

            //check if employee exists in timechimp
            if (employeeHelperTC.EmployeeExists(employeeId))
            {
                return Results.Ok(employeeHelperTC.UpdateEmployee(TCEmployee));
            }
            else
            {
                //check if employee has an emailaddress
                if (TCEmployee.userName == null)
                {
                    return Results.Problem($"Can't create the employee {TCEmployee.displayName} without an emailaddress");
                }

                EmployeeTimeChimp employee = employeeHelperTC.CreateEmployee(TCEmployee);
                TCEmployee.id = employee.id;
                employee = employeeHelperTC.UpdateEmployee(TCEmployee);

                //adds employee to all existing projects in TimeChimp
                projectUserHelperTC.AddAllProjectUserForEmployee(employee.id ?? throw new Exception("User received from timechimp has no id"));

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

            CustomerTimeChimp customer;
            // Find customer id TimeChimpETSContact.CO_KLCOD
            if (ETSProject.PR_KLNR == null)
            {
                customer = customerHelperTC.GetCustomers().Find(c => c.intern != null && c.intern.Value) ?? throw new Exception($"The ETS record for project with id = {projectId} has no customernumber, internal customer is still archived in TimeChimp!");
            }
            else
            {
                customer = customerHelperTC.GetCustomers().Find(c => c.relationId != null && c.relationId.Equals(ETSProject.PR_KLNR)) ?? throw new Exception($"No timechimp cutomer found with id = {ETSProject.PR_KLNR}");
            }
            TCProject.customerId = customer.id ?? throw new Exception("Customer received from timechimp has no id");

            ProjectTimeChimp mainProject = projectHelperTC.FindProject(projectId) ?? projectHelperTC.CreateProject(TCProject);

            List<string> errorMessages = new();
            double totalBudgetHours = 0;
            List<UurcodeTimeChimp> uurcodes = uurcodeHelperTC.GetUurcodes();
            // get subprojects from ETS
            List<SubprojectETS> ETSSubprojects = projectHelperETS.GetSubprojects(projectId);
            foreach (SubprojectETS ETSSubproject in ETSSubprojects.Where(subProject => subProject.SU_SUB != null && !subProject.SU_SUB.StartsWith('2'))) //  only iterate subprojects with ids from [0000, 2000[ en [3000, ...]
            {
                // Change to TimeChimp class
                ProjectTimeChimp TCSubproject = new(ETSSubproject, mainProject)
                {
                    mainProjectId = mainProject.id
                };

                ProjectTimeChimp subProject = projectHelperTC.FindProject(TCSubproject.code ?? throw new Exception("Subproject received from timechimp has no code")) ?? projectHelperTC.CreateProject(TCSubproject);

                TCSubproject.id = subProject.id;
                subProject = projectHelperTC.UpdateProject(TCSubproject);

                //update budgethours for each projecttask in timeChimp
                List<ProjectTaskETS> projectTasksETS = uurcodeHelperETS.GetUurcodesSubproject(ETSProject.PR_NR ?? throw new Exception("Project received from ETS has no NR"), ETSSubproject.SU_SUB ?? throw new Exception("Subproject received from ETS has no SUBNR"));
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
                        UurcodeTimeChimp uurcode = uurcodes.Find(u => u.code != null && u.code.Equals(projectTaskETS.VO_UUR)) ?? throw new Exception($"TimeChimp has no task with code = {projectTaskETS.VO_UUR}");
                        int taskId = uurcode.id ?? throw new Exception("Uurcode in TimeChimp has no id");
                        ProjectTaskTimechimp projectTaskTimechimp = subProject.projectTasks?.Find(p => p.taskId.Equals(taskId)) ?? throw new Exception($"Subproject with code = {subProject.code} has no projecttask with id = {taskId}");

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
            return Results.Ok(timeHelperTC.GetTimes(DateTime.Parse(dateString)));
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
    app.MapGet($"/api/{company.ToLower()}/ets/mileageids", (string dateString) =>
    {
        try
        {
            return Results.Ok(mileageHelperTC.GetApprovedMileageIdsByDate(DateTime.Parse(dateString)));
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

            if (mileage.status == 3)
            {
                throw new Exception($"Mileage with id ({mileageId}) already invoiced");
            }

            string projectNumber = projectHelperTC.GetProject(mileage.projectId).code ?? throw new Exception("Project received from timechimp has no code");
            string employeeNumber = employeeHelperTC.GetEmployee(mileage.userId).employeeNumber ?? throw new Exception("Employee received from timechimp has no employeeNumber");

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
                ((List<Dictionary<string, object?>>) (result["artikels"] ?? throw new Exception("Value at key artikels is null"))).Add(new()
                    {
                        {"artikelNummer", articleHelperETS.GetArticleReference(purchaseOrder.FD_ARTNR ?? throw new Exception($"PurchaseOrder ETS with number = {purchaseOrder.FD_BONNR} has no ARTNR"), header.FH_KLNR ?? throw new Exception($"PurchaseOrder ETS with number = {purchaseOrder.FD_BONNR} has no KLNR"))},
                        {"omschrijving", purchaseOrder.FD_OMS},
                        {"aantal", purchaseOrder.FD_AANTAL},
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
    app.MapGet($"/api/{company.ToLower()}/ets/createpurchasefile", (string id) =>
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
            if (supplier.ToLower().Contains("cebeo"))
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
                            { "aantal", purchaseOrder.FD_AANTAL ?? 0 }
                        });
                    }
                });

                fileContent = purchaseOrderHelperETS.CreateFileCebeo(orderLines, id, supplier, config);
            }
            else
            {
                fileContent = purchaseOrderHelperETS.CreateCSVFile(purchaseOrders, supplier);
            }

            return Results.Ok(fileContent);
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
            string supplierId = supplierHelperETS.FindSupplierId("cebeo");
            List<string> articles = articleHelperETS.GetAricles(supplierId);

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

            Dictionary<string, object> result = new()
            {
                {"families", JsonTool.ConvertTo<List<Dictionary<string, object>>>(responseFamilies)},
                {"subfamilies", JsonTool.ConvertTo<List<Dictionary<string, object>>>(responseSubfamilies)},
                {"measureTypes", JsonTool.ConvertTo<List<Dictionary<string, object>>>(responseMeasureTypes)},
                {"bankAccounts", JsonTool.ConvertTo<List<Dictionary<string, object>>>(responseBankAccounts)},
                {"coinTypes", JsonTool.ConvertTo<List<Dictionary<string, object>>>(responseCoinTypes)},
                {"BTWCodes", JsonTool.ConvertTo<List<Dictionary<string, object>>>(responseBTWCodes)}
            };

            return Results.Ok(result);
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}ArticleFormInfo").WithTags(company);

    app.MapGet($"/api/{company.ToLower()}/ets/validatearticleform", (string articleJSON) =>
    {
        try
        {
            ArticleWeb article = JsonTool.ConvertTo<ArticleWeb>(articleJSON);

            Dictionary<string, string[]> problems = new()
            {
                {"number", Array.Empty<string>() },
                {"reference", Array.Empty<string>() },
                {"description", Array.Empty<string>() },
                {"brand", Array.Empty<string>() },
                {"unitOfMeasure", Array.Empty<string>() },
                {"salesPackQuantity", Array.Empty<string>() },
                {"nettoPrice", Array.Empty<string>() },
                {"tarifPrice", Array.Empty<string>() },
                {"url", Array.Empty<string>() }
            };

            if (string.IsNullOrEmpty(article.Number))
            {
                problems["number"].Append("Can't be empty");
            }
            else if (articleHelperETS.ArticleWithNumberExists(article.Number))
            {
                problems["number"].Append("Articlenumber already exists in ETS");
            }

            if (string.IsNullOrEmpty(article.Reference))
            {
                problems["reference"].Append("Can't be empty");
            }

            if (string.IsNullOrEmpty(article.Description))
            {
                problems["description"].Append("Can't be empty");
            }

            if (string.IsNullOrEmpty(article.Brand))
            {
                problems["brand"].Append("Can't be empty");
            }

            string query = "SELECT EH_COD AS CODE, EH_OMS1 AS DESCRIPTION, EH_OMS2 AS SHORT_DESCRIPTION FROM EENHEID";
            string[] MeasureTypes = JsonTool.ConvertTo<List<Dictionary<string, string>>>(ETSClient.selectQuery(query)).Select(x => x["EH_COD"]).ToArray();
            if (string.IsNullOrEmpty(article.UnitOfMeasure))
            {
                problems["unitOfMeasure"].Append("Can't be empty");
            }
            else if (!MeasureTypes.Contains(article.UnitOfMeasure))
            {
                problems["unitOfMeasure"].Append("Chosen value not valid");
            }

            if (article.SalesPackQuantity < 0)
            {
                problems["salesPackQuantity"].Append("Can't be below zero");
            }

            if (article.NettoPrice < 0)
            {
                problems["nettoPrice"].Append("Price can't be below zero");
            }
            else if (article.TarifPrice < 0)
            {
                problems["tarifPrice"].Append("Price can't be below zero");
            }
            else if (article.TarifPrice < article.NettoPrice)
            {
                problems["nettoPrice"].Append("Price can't be higher then tarif price");
                problems["tarifPrice"].Append("Price can't be lower then netto price");
            }

            if (string.IsNullOrEmpty(article.URL))
            {
                problems["url"].Append("Can't be empty");
            }


            if (problems.Count > 0)
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

    app.MapPost($"/api/{company.ToLower()}/ets/transformbomexcel", ([FromBody] OwnFileContentResult excelFile) =>
    {
        try
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var stream = new MemoryStream(excelFile.FileContents ?? throw new Exception("File given has no content")))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    DataSet data = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

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
                                #pragma warning restore CS8602 // Dereference of a possibly null reference.
                            }
                            else
                            {
                                Item part = new((string)row["Part Number"], (string)row["Description"], Convert.ToInt32((double)row["QTY"]));

                                if (part.Number.Length > 25)
                                {
                                    throw new Exception($"Excel contains an item where the part number is longer then 25 characters \"{part.Number}\", this is not allowed");
                                }

                                part.ExistsETS = articleHelperETS.ArticleWithNumberExists(part.Number);

                                parentList[level] = part;
                            }
                        }
                    }
                    return Results.Ok(assemblies);
                }
            }
        }
        catch (Exception e)
        {
            return Results.Problem(e.Message);
        }
    }).WithName($"{company}TransformBomExcel").WithTags(company);

    app.MapPut($"/api/{company.ToLower()}/ets/updatelinkedarticles", ([FromBody] List<Item> articles) =>
    {
        void LinkArticles(Item main)
        {
            foreach (Item part in main.Parts.Where(i => i != null).Cast<Item>().ToList())
            {
                articleHelperETS.LinkArticle(main.Number, part.Number);
                LinkArticles(part);
            }
        };

        articles.ForEach(a => LinkArticles(a));
    });
}

app.MapGet("/api/companies", () => Results.Ok(companies)).WithName($"GetCompanyNames");

if (app.Environment.IsProduction())
{
    app.Run("http://*:5000");
} 
else
{
    app.Run();
}