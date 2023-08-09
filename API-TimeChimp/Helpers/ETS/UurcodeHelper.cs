namespace Api.Devion.Helpers.ETS;

public class ETSUurcodeHelper : ETSHelper
{
    public ETSUurcodeHelper(FirebirdClientETS FBClient) : base(FBClient)
    {
    }


    public List<string> GetUurcodes(DateTime date)
    {
        var query = $"SELECT UR_COD, UR_OMS FROM URPX WHERE UR_COD LIKE '0%' AND DATE_CHANGED >= '{date.ToString("MM / dd / yyyy HH: mm")}'";

        //get data from ETS
        var response = ETSClient.selectQuery(query);
        //convert data to uurcodesETS object
        List<uurcodesETS> uurcodes = JsonTool.ConvertTo<List<uurcodesETS>>(response);

        //create list with all uurcodeids
        List<string> uurcodesIds = new List<string>();
        foreach (uurcodesETS uurcode in uurcodes)
        {
            uurcodesIds.Add(uurcode.UR_COD);
        }
        return uurcodesIds;
    }

    public uurcodesETS GetUurcode(string uurcodeId)
    {
        var query = $"SELECT UR_COD, UR_OMS FROM URPX WHERE UR_COD = '{uurcodeId}'";

        //get data from ETS
        var response = ETSClient.selectQuery(query);
        Console.WriteLine(response);
        //convert data to uurcodesETS object
        uurcodesETS uurcode = JsonTool.ConvertTo<List<uurcodesETS>>(response).FirstOrDefault();

        return uurcode;
    }
}