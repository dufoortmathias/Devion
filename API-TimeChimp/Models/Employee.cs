namespace Api.Devion.Models
{
    public class EmployeeTimeChimp
    {
        public Int32? id { get; set; }
        public String? userName { get; set; }
        public String? displayName { get; set; }
        public Int32? accountType { get; set; }
        public Boolean? isLocked { get; set; }
        public String? picture { get; set; }
        public String[]? tagNames { get; set; }
        public String? language { get; set; }
        public Double? contractHours { get; set; }
        public Double? contractHourlyRate { get; set; }
        public Double? contractCostHourlyRate { get; set; }
        public DateTime? contractStartDate { get; set; }
        public DateTime? contractEndDate { get; set; }
        public DateTime? created { get; set; }
        public String? teamName { get; set; }
        public String? employeeNumber { get; set; }
        public Boolean? active { get; set; }
        public String? contractNumber { get; set; }
        public String? badgeNumber { get; set; }
        public Int32? roleId { get; set; }
        public String? citizenServiceNumber { get; set; }

        public EmployeeTimeChimp() { }

        public EmployeeTimeChimp(EmployeeETS employee)
        {
            this.employeeNumber = employee.PN_ID;
            this.userName = employee.PN_USR;
            this.displayName = employee.PN_NAM;
            this.active = employee.PN_AKTIEF == 1 ? true : false;
            this.badgeNumber = employee.PN_BADGENR;
            this.language = "nl";
            this.employeeNumber = employee.PN_ID;
            this.accountType = 1;
            this.tagNames = new string[] { };
        }
    }

    public class EmployeeETS
    {
        public String? PN_ID { get; set; }
        public String? PN_EMAIL { get; set; }
        public String? PN_NAM { get; set; }
        public string? PN_USR { get; set; }
        public String? PN_TAAL { get; set; }
        public int? PN_AKTIEF { get; set; }
        public String? PN_BADGENR { get; set; }
    }
}
