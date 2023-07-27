namespace Api.Devion.Helpers.ETS;

public static class ETSTimeHelper
{
    public static List<timeETS> GetTime()
    {
        var client = new FirebirdClientETS();
        var response = client.selectQuery("select * from tbl_planning");
        List<timeETS> times = JsonConvert.DeserializeObject<List<timeETS>>(response);
        foreach (timeETS time in times)
        {
            string query = $"select * from J2W_PNPX where PN_ID = {time.PLA_PERSOON}";
            string json = client.selectQuery(query);
            Console.WriteLine(json);
            List<naamTimeETS> naam = JsonConvert.DeserializeObject<List<naamTimeETS>>(json);
            time.PN_NAM = naam.First().PN_NAM;
        }
        return times;
    }


}