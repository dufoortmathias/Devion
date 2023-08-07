namespace Api.Devion.Models
{
    public class EmployeeTimeChimp
    {
        public Int32? id { get; set; }
        public String? email { get; set; }
        public String userName { get; set; }
        public String displayName { get; set; }
        public Int32? accountType { get; set; }
        public Boolean? isLocked { get; set; }
        public String? picture { get; set; }
        public String?[] tagNames { get; set; }
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
        public Boolean? sendInvitation { get; set; }

        public EmployeeTimeChimp() { }

        public EmployeeTimeChimp(EmployeeETS employeeETS)
        {
            employeeNumber = employeeETS.PN_ID;
            userName = employeeETS.PN_EMAIL;
            displayName = employeeETS.PN_NAM;
            language = employeeETS.PN_TAAL switch
            {
                'N' => "nl",
                'F' => "fr",
                'E' => "en",
                'D' => "de",
                _ => "nl"
            };
            badgeNumber = employeeETS.PN_BADGENR;
            roleId = 1; //TODO: add value to seperate file
            tagNames = new String?[] { };
        }
    }

    public class EmployeeETS
    {
        public String? PN_ID { get; set; }
        public String? PN_NAM { get; set; }
        public String? PN_EMAIL { get; set; }
        public String? PN_WERKNEMERSNR { get; set; }
        public String? PN_BADGENR { get; set; }
        public Char? PN_TAAL { get; set; }
    }
}
