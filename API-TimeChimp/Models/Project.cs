namespace Api.Devion.Models
{
    public class ProjectTimeChimp
    {
        public Double? remainingBudgetHours { get; set; }
        public Int32[]? tagIds { get; set; }
        public String[]? tagNames { get; set; }
        public Boolean? unspecified { get; set; }
        public DateTime? invoiceDate { get; set; }
        public Boolean? invoiceInInstallments { get; set; }
        public Double? budgetNotificationPercentage { get; set; }
        public Boolean? budgetNotificationHasBeenSent { get; set; }
        public String? clientId { get; set; }
        public Int32? invoiceStatus { get; set; }
        public Int32? invoiceId { get; set; }
        public String? color { get; set; }
        public Boolean? visibleOnSchedule { get; set; }
        public String? externalUrl { get; set; }
        public String? externalName { get; set; }
        public String? invoiceReference { get; set; }
        public Object[]? projectTasks { get; set; }
        public Object[]? projectUsers { get; set; }
        public Int32? id { get; set; }
        public Boolean? active { get; set; }
        public Int32 customerId { get; set; }
        public String? customerName { get; set; }
        public String? name { get; set; }
        public String? code { get; set; }
        public String? notes { get; set; }
        public Int32 invoiceMethod { get; set; }
        public Double? hourlyRate { get; set; }
        public Double? rate { get; set; }
        public Int32 budgetMethod { get; set; }
        public Double? budgetRate { get; set; }
        public Double? budgetHours { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public Object? projectSubscription { get; set; }
        public DateTime? modified { get; set; }
        public Int32? mainProjectId { get; set; }
        public Int32[]? projectManagerIds { get; set; }
        public Boolean? useSubprojects { get; set; }
        public Int32[]? subprojectIds { get; set; }

        public ProjectTimeChimp() {}

        public ProjectTimeChimp(ProjectETS projectETS)
        {
            code = projectETS.PR_NR;
            name = projectETS.PR_KROM;
            customerId = TimeChimpCustomerHelper.GetCustomers().Find(c => c.relationId != null && c.relationId.Equals(projectETS.PR_KLNR)).id.Value;
            startDate = projectETS.PR_START_PRODUCTIE;
            endDate = projectETS.PR_BELOOFD;
            active = projectETS.PR_STAT.Equals('A');
            useSubprojects = true; //TODO: add value to seperate file
            invoiceMethod = 1; //TODO: add value to seperate file
            budgetMethod = 2; //TODO: add value to seperate file
        }

        public ProjectTimeChimp(SubprojectETS subprojectETS)
        {
            ProjectTimeChimp mainProject = TimeChimpProjectHelper.GetProjects().Find(p => p.code != null && p.code.Equals(subprojectETS.SU_NR));

            code = subprojectETS.VOLNR;
            name = subprojectETS.SU_OMS;
            mainProjectId = mainProject.id;
            customerId = mainProject.customerId;
            startDate = mainProject.startDate;
            endDate = mainProject.endDate;
            active = subprojectETS.SU_AFGEWERKT == 1;
            useSubprojects = false; //TODO: add value to seperate file
            invoiceMethod = 1; //TODO: add value to seperate file
            budgetMethod = 2; //TODO: add value to seperate file
        }
    }

    public class ProjectETS
    {
        public String PR_NR { get; set; }
        public String PR_KLNR { get; set; }
        public String PR_KROM { get; set; }
        public DateTime? PR_START_PRODUCTIE { get; set; }
        public DateTime? PR_BELOOFD { get; set; }
        public Char? PR_STAT { get; set; }
    }

    public class SubprojectETS
    {
        public String SU_NR { get; set; }
        public String SU_SUB { get; set; }
        public String SU_OMS { get; set; }
        public String VOLNR { get; set; }
        public Int32? SU_AFGEWERKT { get; set; }
    }
}
