namespace Api.Devion.Models
{
    public class ProjectTimeChimp
    {
        public double? remainingBudgetHours { get; set; }
        public int[]? tagIds { get; set; }
        public string[]? tagNames { get; set; }
        public bool? unspecified { get; set; }
        public DateTime? invoiceDate { get; set; }
        public bool? invoiceInInstallments { get; set; }
        public double? budgetNotificationPercentage { get; set; }
        public bool? budgetNotificationHasBeenSent { get; set; }
        public string? clientId { get; set; }
        public int? invoiceStatus { get; set; }
        public int? invoiceId { get; set; }
        public string? color { get; set; }
        public bool? visibleOnSchedule { get; set; }
        public string? externalUrl { get; set; }
        public string? externalName { get; set; }
        public string? invoiceReference { get; set; }
        public object[]? projectTasks { get; set; }
        public object[]? projectUsers { get; set; }
        public int? id { get; set; }
        public bool? active { get; set; }
        public int customerId { get; set; }
        public string? customerName { get; set; }
        public string? name { get; set; }
        public string? code { get; set; }
        public string? notes { get; set; }
        public int invoiceMethod { get; set; }
        public double? hourlyRate { get; set; }
        public double? rate { get; set; }
        public int budgetMethod { get; set; }
        public double? budgetRate { get; set; }
        public double? budgetHours { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public object? projectSubscription { get; set; }
        public DateTime? modified { get; set; }
        public int? mainProjectId { get; set; }
        public int[]? projectManagerIds { get; set; }
        public bool? useSubprojects { get; set; }
        public int[]? subprojectIds { get; set; }

        //constructor without specific parameters
        public ProjectTimeChimp() { }

        //constructor to from timechimp class to ets class (mainproject)
        public ProjectTimeChimp(ProjectETS projectETS)
        {
            code = projectETS.PR_NR;
            name = projectETS.PR_KROM;
            startDate = projectETS.PR_START_PRODUCTIE;
            active = projectETS.PR_STAT.Equals('L');
            useSubprojects = true; //TODO: add value to seperate file
            invoiceMethod = 2; //TODO: add value to seperate file
            budgetMethod = 1; //TODO: add value to seperate file
        }

        //constructor to from timechimp class to ets class (subproject)
        public ProjectTimeChimp(SubprojectETS subprojectETS, ProjectTimeChimp mainProject)
        {
            if (subprojectETS != null && mainProject != null)
            {
                code = subprojectETS.VOLNR;
                name = subprojectETS.SU_OMS;
                customerId = mainProject.customerId;
                startDate = subprojectETS.SU_START_PRODUCTIE;
                active = subprojectETS.SU_AFGEWERKT != 1;
                useSubprojects = false; //TODO: add value to seperate file
                invoiceMethod = 2; //TODO: add value to seperate file
                budgetMethod = 1; //TODO: add value to seperate file
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
        public char? PR_STAT { get; set; }
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
}
