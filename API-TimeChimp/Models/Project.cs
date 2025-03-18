namespace Api.Devion.Models
{
    public class ProjectTimeChimp
    {
        public int Id { get; set; }
        public bool? Active { get; set; }
        public bool? Unspecified { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Notes { get; set; }
        public string? Color { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public Invoicing? Invoicing { get; set; }
        public Budget? Budget { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public CustomerTimeChimp? Customer { get; set; }
        public Project? MainProject { get; set; }
        public Project[]? SubProjects { get; set; }
        public List<ManagerTC> Managers { get; set; }
        public ProjectTaskTC[]? ProjectTasks { get; set; }
        public List<ProjectTaskTC> ProjectTaskList { get; set; }
        public ProjectUserTC[]? ProjectUsers { get; set; }
        public Tag[]? Tags { get; set; }

        //constructor without specific parameters
        public ProjectTimeChimp() { }

        //constructor to from timechimp class to ets class (mainproject)
        public ProjectTimeChimp(ProjectETS projectETS)
        {
            Code = projectETS.PR_NR;
            Name = projectETS.PR_KROM;
            StartDate = projectETS.PR_START_PRODUCTIE.ToString();
            Console.WriteLine(projectETS.PR_STAT + "luk");
            Active = projectETS.PR_STAT.Contains('L');
            SubProjects = Array.Empty<Project>();
            Invoicing = new();
            Budget = new();
            Invoicing.Method = "TaskHourlyRate"; //TODO: add value to seperate file
            Budget.Method = "TotalHours"; //TODO: add value to seperate file
            Managers = new();
        }

        //constructor to from timechimp class to ets class (subproject)
        public ProjectTimeChimp(SubprojectETS subprojectETS, ProjectTimeChimp mainProject)
        {
            if (subprojectETS != null && mainProject != null)
            {
                Code = subprojectETS.VOLNR;
                Name = subprojectETS.SU_OMS;
                Customer = new();
                Customer.Id = mainProject.Customer.Id;
                StartDate = subprojectETS.SU_START_PRODUCTIE.ToString();
                Active = subprojectETS.SU_AFGEWERKT != 1;
                MainProject = new()
                {
                    Id = mainProject.Id
                };
                Invoicing = new()
                {
                    Method = "TaskHourlyRate" //TODO: add value to seperate file
                };
                Budget = new()
                {
                    Method = "TaskHours" //TODO: add value to seperate file
                };
                ProjectTaskList = new();
                Managers = new();
            }
        }
    }
    public class ProjectETS
    {
        public string? PR_NR { get; set; }
        public string? PR_KLNR { get; set; }
        public string? PR_KROM { get; set; }
        public DateTime? PR_START_PRODUCTIE { get; set; }
        public DateTime? PR_BELOOFD { get; set; }
        public string? PR_STAT { get; set; }
        public string? PR_BESTEMMELING { get; set; }
    }

    public class SubprojectETS
    {
        public string? SU_NR { get; set; }
        public string? SU_SUB { get; set; }
        public string? SU_OMS { get; set; }
        public string? VOLNR { get; set; }
        public int? SU_AFGEWERKT { get; set; }
        public DateTime? SU_START_PRODUCTIE { get; set; }
    }

    public class Project
    {
        public int? Id { get; set; }
        public bool? Active { get; set; }
        public bool? Unspecified { get; set; }
        public string? Name { get; set; }
        public string? Color { get; set; }
    }

    public enum BudgetMethod
    {
        NoBudget,
        TotalHours,
        TaskHours,
        UserHours,
        TotalRate,
        TaskRate,
        Invoice,
        TotalCost
    }

    public enum InvoiceMethod
    {
        NoInvoicing,
        TaskHourlyRate,
        UserHourlyRate,
        ProjectHourlyRate,
        CustomerHourlyRate,
        ProjectRate,
        TaskRate,
        Subscription
    }

    public class Budget
    {
        public String? Method { get; set; }
        public float? Hours { get; set; }
        public float? Rate { get; set; }
        public float? NotificationPercentage { get; set; }
    }

    public class Manager
    {
        public int? Id { get; set; }
        public bool? Active { get; set; }
        public bool? Unspecified { get; set; }
        public string? UserName { get; set; }
        public string? DisplayName { get; set; }
    }

    public class Invoicing
    {
        public string? Method { get; set; }
        public float? HourlyRate { get; set; }
        public float? FixedRate { get; set; }
        public string? Reference { get; set; }
        public string? Date { get; set; }
    }

    public class TaskTC
    {
        public int Id { get; set; }
        public bool? Active { get; set; }
        public bool? Unspecified { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
    }
    public class ProjectTaskTC
    {
        public int? Id { get; set; }
        public bool? Active { get; set; }
        public bool? Billable { get; set; }
        public float? HourlyRate { get; set; }
        public float? FixedRate { get; set; }
        public float? BudgetHours { get; set; }
        public float? BudgetRate { get; set; }
        public TaskTC? Task { get; set; }
    }

    public class ProjectUserTC
    {
        public int? Id { get; set; }
        public bool? Active { get; set; }
        public float? HourlyRate { get; set; }
        public float? BudgetHours { get; set; }
        public UserTC? User { get; set; }
    }

    public class UserTC
    {
        public int Id { get; set; }
        public bool? Active { get; set; }
        public string? UserName { get; set; }
        public string? DisplayName { get; set; }
    }

    public class ResponseTCProject
    {
        public ProjectTimeChimp[]? Result { get; set; }
        public Link[]? Links { get; set; }
        public int? Count { get; set; }
    }

    public class ManagerTC
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public string? UserName { get; set; }
        public string? DisplayName { get; set; }
    }
}
