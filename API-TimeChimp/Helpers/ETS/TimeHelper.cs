namespace Api.Devion.Helpers.ETS;

public static class ETSTimeHelper
{
    public static List<timeETS> GetTime()
    {
        var client = new FirebirdClientETS();
        var response = client.selectQuery("select * from tbl_planning");
        List<timeETS> times = JsonTool.ConvertTo<List<timeETS>>(response);
        foreach (timeETS time in times)
        {
            string query = $"select * from J2W_PNPX where PN_ID = {time.PLA_PERSOON}";
            string json = client.selectQuery(query);
            List<naamTimeETS> naam = JsonTool.ConvertTo<List<naamTimeETS>>(json);
            time.PN_NAM = naam.First().PN_NAM;
        }
        return times;
    }

    public static List<timeETS> addTimesETS(List<timeETS> times)
    {
        var client = new FirebirdClientETS();

        foreach (timeETS time in times)
        {
            var response = client.insertQuery($"insert into tbl_planning (PLA_CAPTION, PLA_START, PLA_EINDE, PLA_KM_PAUZE, PLA_TEKST, PLA_PROJECT, PLA_SUBPROJECT, PLA_PERSOON) values ({time.PLA_CAPTION}, {time.PLA_START.Value.ToString("yyyy-MM-dd HH:mm:ss")}, {time.PLA_EINDE.Value.ToString("yyyy-MM-dd HH:mm:ss")}, {time.PLA_KM_PAUZE}, {time.PLA_TEKST}, {time.PLA_PROJECT}, {time.PLA_SUBPROJECT}, {time.PLA_PERSOON})");
        }
        return times;
    }

    public static string addTime(timeETS time)
    {
        var client = new FirebirdClientETS();
        var response = client.selectQuery("select max(PLA_ID) from tbl_planning");

        List<maxValue> max = JsonTool.ConvertTo<List<maxValue>>(response);
        if (max[0].MAX == null)
        {
            time.PLA_ID = 1;
        }
        else
        {
            time.PLA_ID = max[0].MAX + 1;
        }
        customerTimeChimp customer = TimeChimpCustomerHelper.GetCustomer(time.PLA_KLANT.ToString());
        time.PLA_KLANT = customer.relationId;
        response = client.insertQuery($"INSERT INTO tbl_planning (PLA_ID, PLA_KLEUR, PLA_CAPTION, PLA_START, PLA_EINDE, PLA_KM_PAUZE, PLA_TEKST, PLA_PROJECT, PLA_SUBPROJECT, PLA_PERSOON, PLA_KLANT, PLA_UURCODE) " +
                                        $"VALUES ({time.PLA_ID}, {time.PLA_KLEUR}, '{time.PLA_CAPTION}', '{time.PLA_START.Value.ToString("yyyy-MM-dd HH:mm:ss")}', " +
                                        $"'{time.PLA_EINDE.Value.ToString("yyyy-MM-dd HH:mm:ss")}', '{time.PLA_KM_PAUZE}', '{time.PLA_TEKST}', " +
                                        $"'{time.PLA_PROJECT}', '{time.PLA_SUBPROJECT}', '{time.PLA_PERSOON}', '{time.PLA_KLANT}', '{time.PLA_UURCODE}')");
        return response;
    }

    public static List<timeETS> addTimes()
    {
        List<timeETS> times = TimeChimpTimeHelper.GetTimesLastWeek();
        List<int> ids = new List<int>();
        foreach (timeETS time in times)
        {
            var response = addTime(time);
            ids.Add(time.timechimpId);
        }
        changeRegistrationStatusTimeChimp status = TimeChimpTimeHelper.changeStatus(ids);
        return times;
    }
}