namespace Api.Devion.Helpers.ETS;

public class ETSUurcodeHelper : ETSHelper
{
    public ETSUurcodeHelper(FirebirdClientETS FBClient) : base(FBClient)
    {
    }


    public List<uurcodesETS> GetUurcodes(DateTime date)
    {
        var query = $"SELECT UR_COD, UR_OMS FROM URPX WHERE UR_COD LIKE '0%' AND DATE_CHANGED >= '{date.ToString("MM / dd / yyyy HH: mm")}'";

        //get data from ETS
        var response = ETSClient.selectQuery(query);
        //convert data to uurcodesETS object
        List<uurcodesETS> uurcodes = JsonTool.ConvertTo<List<uurcodesETS>>(response);

        return uurcodes;
    }

    public uurcodesETS GetUurcode(string uurcodeId)
    {
        var query = $"SELECT UR_COD, UR_OMS FROM URPX WHERE UR_COD = '{uurcodeId}'";

        //get data from ETS
        var response = ETSClient.selectQuery(query);
        //convert data to uurcodesETS object
        uurcodesETS uurcode = JsonTool.ConvertTo<uurcodesETS>(response);

        return uurcode;
    }
}