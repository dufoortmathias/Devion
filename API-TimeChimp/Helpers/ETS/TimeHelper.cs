namespace Api.Devion.Helpers.ETS;

public class ETSTimeHelper : ETSHelper
{
    private BearerTokenHttpClient TCClient;
    public ETSTimeHelper(FirebirdClientETS FBClient, BearerTokenHttpClient clientTC) : base(FBClient)
    {
        TCClient = clientTC;
    }

    public List<timeETS> GetTime()
    {
        var response = ETSClient.selectQuery("select * from tbl_planning");
        List<timeETS> times = JsonTool.ConvertTo<List<timeETS>>(response);
        foreach (timeETS time in times)
        {
            string query = $"select * from J2W_PNPX where PN_ID = {time.PLA_PERSOON}";
            string json = ETSClient.selectQuery(query);
            List<naamTimeETS> naam = JsonTool.ConvertTo<List<naamTimeETS>>(json);
            time.PN_NAM = naam.First().PN_NAM;
        }
        return times;
    }

    public List<timeETS> addTimesETS(List<timeETS> times)
    {
        foreach (timeETS time in times)
        {
            var response = ETSClient.insertQuery($"insert into tbl_planning (PLA_CAPTION, PLA_START, PLA_EINDE, PLA_KM_PAUZE, PLA_TEKST, PLA_PROJECT, PLA_SUBPROJECT, PLA_PERSOON) values ({time.PLA_CAPTION}, {time.PLA_START.Value.ToString("yyyy-MM-dd HH:mm:ss")}, {time.PLA_EINDE.Value.ToString("yyyy-MM-dd HH:mm:ss")}, {time.PLA_KM_PAUZE}, {time.PLA_TEKST}, {time.PLA_PROJECT}, {time.PLA_SUBPROJECT}, {time.PLA_PERSOON})");
        }
        return times;
    }

    public string addTime(timeETS time)
    {
        var response = ETSClient.selectQuery("select max(PLA_ID) from tbl_planning");

        List<maxValue> max = JsonTool.ConvertTo<List<maxValue>>(response);
        if (max[0].MAX == null)
        {
            time.PLA_ID = 1;
        }
        else
        {
            time.PLA_ID = max[0].MAX + 1;
        }
        customerTimeChimp customer =  new TimeChimpCustomerHelper(TCClient).GetCustomer(time.PLA_KLANT.ToString());
        time.PLA_KLANT = customer.relationId;
        response = ETSClient.insertQuery($"INSERT INTO tbl_planning (PLA_ID, PLA_KLEUR, PLA_CAPTION, PLA_START, PLA_EINDE, PLA_KM_PAUZE, PLA_TEKST, PLA_PROJECT, PLA_SUBPROJECT, PLA_PERSOON, PLA_KLANT, PLA_UURCODE) " +
                                        $"VALUES ({time.PLA_ID}, {time.PLA_KLEUR}, '{time.PLA_CAPTION}', '{time.PLA_START.Value.ToString("yyyy-MM-dd HH:mm:ss")}', " +
                                        $"'{time.PLA_EINDE.Value.ToString("yyyy-MM-dd HH:mm:ss")}', '{time.PLA_KM_PAUZE}', '{time.PLA_TEKST}', " +
                                        $"'{time.PLA_PROJECT}', '{time.PLA_SUBPROJECT}', '{time.PLA_PERSOON}', '{time.PLA_KLANT}', '{time.PLA_UURCODE}')");
        return response;
    }

    public List<timeETS> addTimes()
    {
        List<timeETS> times = new TimeChimpTimeHelper(TCClient, ETSClient).GetTimesLastWeek();
        List<int> ids = new List<int>();
        foreach (timeETS time in times)
        {
            var response = addTime(time);
            ids.Add(time.timechimpId);
        }
        changeRegistrationStatusTimeChimp status = new TimeChimpTimeHelper(TCClient, ETSClient).changeStatus(ids);
        return times;
    }
}