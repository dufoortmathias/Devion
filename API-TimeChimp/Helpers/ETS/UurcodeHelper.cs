namespace Api.Devion.Helpers.ETS;

public class ETSUurcodeHelper
{
    public static List<uurcodesETS> GetUurcodes()
    {
        // connection with ETS
        var client = new FirebirdClientETS();

        var query = "SELECT UR_COD, UR_OMS FROM URPX WHERE UR_COD LIKE '0%'";

        //get data from ETS
        var response = client.selectQuery(query);
        //convert data to uurcodesETS object
        List<uurcodesETS> uurcodes = JsonTool.ConvertTo<List<uurcodesETS>>(response);

        return uurcodes;
    }
}