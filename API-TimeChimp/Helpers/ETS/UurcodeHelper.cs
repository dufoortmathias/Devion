namespace Api.Devion.Helpers.ETS;

public class ETSUurcodeHelper
{
    public static String[] GetUurcodes(DateTime date)
    {
        // connection with ETS
        var client = new FirebirdClientETS();

        var query = $"SELECT UR_COD, UR_OMS FROM URPX WHERE UR_COD LIKE '0%' and DATE_CHANGED BETWEEN '{date.ToString("MM / dd / yyyy HH: mm")}' AND '{DateTime.Now.ToString("MM / dd / yyyy HH: mm")}'";

        //get data from ETS
        string json = client.selectQuery(query);
        String[] ids = JsonTool.ConvertTo<ProjectETS[]>(json)
            .Select(project => project.PR_NR)
            .ToArray();
        return ids;
    }

    public static uurcodesETS GetUurcode(string uurcodeId)
    {
        // connection with ETS
        var client = new FirebirdClientETS();

        var query = $"SELECT UR_COD, UR_OMS FROM URPX WHERE UR_COD = '{uurcodeId}'";

        //get data from ETS
        var response = client.selectQuery(query);

        //convert data to uurcodesETS object
        uurcodesETS uurcode = JsonTool.ConvertTo<uurcodesETS>(response);

        return uurcode;
    }
}