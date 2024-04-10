namespace Api.Devion.Models
{
    public class EmployeeTimeChimp
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? DisplayName { get; set; }
        public bool? IsLocked { get; set; }
        public bool? Active { get; set; }
        public string? Picture { get; set; }
        public string? Language { get; set; }
        public string? EmployeeNumber { get; set; }
        public string? BadgeNumber { get; set; }
        public string? CitizenServiceNumber { get; set; }
        public string? Created { get; set; }
        public string? Modified { get; set; }
        public Role? Role { get; set; }
        public Team? Team { get; set; }
        public List<Contract>? Contracts { get; set; }
        public bool? SendInvitation { get; set; }

        //constructor without specific parameters
        public EmployeeTimeChimp() { }

        //constructor to from ets class to timechimp class
        public EmployeeTimeChimp(EmployeeETS employeeETS, int roleId)
        {
            EmployeeNumber = employeeETS.PN_ID;
            UserName = employeeETS.PN_EMAIL;
            DisplayName = employeeETS.PN_NAM;
            Language = employeeETS.PN_TAAL switch
            {
                'N' => "nl",
                'F' => "fr",
                'E' => "en",
                'D' => "de",
                _ => "nl"
            };
            BadgeNumber = employeeETS.PN_BADGENR;
            Role = new Role
            {
                Id = roleId //TODO: add value to seperate file
            };
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

    public class ResponseTCEmployee
    {
        public EmployeeTimeChimp[]? Result { get; set; }
        public Link[]? Links { get; set; }
        public int? Count { get; set; }
    }
}
