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

    public static List<timeETS> addTimesETS(List<timeETS> times)
    {
        var client = new FirebirdClientETS();

        foreach (timeETS time in times)
        {
            var response = client.insertQuery($"insert into tbl_planning (PLA_CAPTION, PLA_START, PLA_EINDE, PLA_KM_PAUZE, PLA_TEKST, PLA_PROJECT, PLA_SUBPROJECT, PLA_PERSOON) values ({time.PLA_CAPTION}, {time.PLA_START}, {time.PLA_EINDE}, {time.PLA_KM_PAUZE}, {time.PLA_TEKST}, {time.PLA_PROJECT}, {time.PLA_SUBPROJECT}, {time.PLA_PERSOON})");
        }
        return times;
    }

    public static List<timeETS> addTimes()
    {
        List<timeETS> times = TimeChimpTimeHelper.GetTimesLastWeek();
        times = addTimesETS(times);
        return times;
    }
}