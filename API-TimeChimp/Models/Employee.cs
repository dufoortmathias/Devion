namespace Api.Devion.Models
{
    public class EmployeeTimeChimp
    {
        public int? id { get; set; }
        public string? email { get; set; }
        public string userName { get; set; }
        public string displayName { get; set; }
        public int? accountType { get; set; }
        public bool? isLocked { get; set; }
        public string? picture { get; set; }
        public string?[] tagNames { get; set; }
        public string? language { get; set; }
        public double? contractHours { get; set; }
        public double? contractHourlyRate { get; set; }
        public double? contractCostHourlyRate { get; set; }
        public DateTime? contractStartDate { get; set; }
        public DateTime? contractEndDate { get; set; }
        public DateTime? created { get; set; }
        public string? teamName { get; set; }
        public string? employeeNumber { get; set; }
        public bool? active { get; set; }
        public string? contractNumber { get; set; }
        public string? badgeNumber { get; set; }
        public int? roleId { get; set; }
        public string? citizenServiceNumber { get; set; }
        public bool? sendInvitation { get; set; }

        //constructor without specific parameters
        public EmployeeTimeChimp() { }

        //constructor to from ets class to timechimp class
        public EmployeeTimeChimp(EmployeeETS employeeETS, int roleId)
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
            this.roleId = roleId; //TODO: add value to seperate file
            tagNames = new string?[] { };
        }
    }

    public class EmployeeETS
    {
        public string? PN_ID { get; set; }
        public string? PN_NAM { get; set; }
        public string? PN_EMAIL { get; set; }
        public string? PN_WERKNEMERSNR { get; set; }
        public string? PN_BADGENR { get; set; }
        public char? PN_TAAL { get; set; }
    }
}
