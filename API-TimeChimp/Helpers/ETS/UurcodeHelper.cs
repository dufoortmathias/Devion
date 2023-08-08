namespace Api.Devion.Helpers.ETS;

public class ETSUurcodeHelper : ETSHelper
{
    public ETSUurcodeHelper(FirebirdClientETS FBClient) : base(FBClient)
    {
    }

    public List<uurcodesETS> GetUurcodes()
    {
        var query = "SELECT UR_COD, UR_OMS FROM URPX WHERE UR_COD LIKE '0%'";

        //get data from ETS
        var response = ETSClient.selectQuery(query);
        //convert data to uurcodesETS object
        List<uurcodesETS> uurcodes = JsonTool.ConvertTo<List<uurcodesETS>>(response);

        return uurcodes;
    }
}