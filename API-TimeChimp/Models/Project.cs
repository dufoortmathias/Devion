namespace api_Devion.Models
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
        public String name { get; set; }
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
    }
}
