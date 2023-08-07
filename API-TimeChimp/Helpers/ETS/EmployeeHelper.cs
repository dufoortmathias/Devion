namespace Api.Devion.Helpers.ETS
{
    public class ETSEmployeeHelper
    {
        public static String[] GetEmployeeIdsChangedAfter(DateTime date)
        {
            FirebirdClientETS client = new FirebirdClientETS();
            string query = $"SELECT PN_ID FROM J2W_PNPX WHERE DATE_CHANGED BETWEEN '{date.ToString("MM/dd/yyyy HH:mm")}' AND '{DateTime.Now.ToString("MM/dd/yyyy HH:mm")}'";
            string json = client.selectQuery(query);
            String[] ids = JsonTool.ConvertTo<EmployeeETS[]>(json)
                .Select(contact => contact.PN_ID)
                .ToArray();
            return ids;
        }

        public static EmployeeETS GetEmployee(String employeeId)
        {
            FirebirdClientETS client = new FirebirdClientETS();
            string query = $"SELECT * FROM J2W_PNPX WHERE PN_ID = {employeeId}";
            string json = client.selectQuery(query);
            EmployeeETS employee = JsonTool.ConvertTo<EmployeeETS[]>(json).FirstOrDefault();
            return employee;
        }
    }
}
