namespace Api.Devion.Helpers.ETS;

public class ETSUurcodeHelper : ETSHelper
{
    public ETSUurcodeHelper(FirebirdClientETS FBClient) : base(FBClient)
    {
    }

    // get all uurcodes that are changed after the given date
    public List<string> GetUurcodes(DateTime date)
    {
        string query = $"SELECT UR_COD, UR_OMS FROM URPX WHERE UR_COD LIKE '0%' AND DATE_CHANGED >= @date";
        Dictionary<string, object> parameters = new()
        {
            {"@date",  date}
        };

        //get data from ETS
        string response = ETSClient.selectQuery(query, parameters);

        //check if json is not empty
        if (response == null)
        {
            throw new Exception("Error getting uurcodes from ETS with query: " + query);
        }

        //convert data to uurcodesETS object
        List<UurcodeETS> uurcodes = JsonTool.ConvertTo<List<UurcodeETS>>(response);

        //create list with all uurcodeids
        List<string> uurcodesIds = uurcodes
            .Select(uc => uc.UR_COD)
            .Where(x => x != null)
            .Cast<string>()
            .ToList();

        return uurcodesIds;
    }

    // get uurcode by uurcodeId
    public UurcodeETS? GetUurcode(string uurcodeId)
    {
        string query = $"SELECT UR_COD, UR_OMS FROM URPX WHERE UR_COD = @uurcode";
        Dictionary<string, object> parameters = new()
        {
            {"@uurcode",  uurcodeId}
        };

        //get data from ETS
        string response = ETSClient.selectQuery(query, parameters);

        //check if json is not empty
        if (response == null)
        {
            throw new Exception("Error getting uurcode from ETS with query: " + query);
        }

        //convert data to uurcodesETS object
        UurcodeETS? uurcode = JsonTool.ConvertTo<List<UurcodeETS>>(response).FirstOrDefault();

        return uurcode;
    }

    public List<ProjectTaskETS> GetUurcodesSubproject(string projectId, string subprojectId)
    {
        string query = $"SELECT VO_PROJ, VO_SUBPROJ, VO_UUR, SUM(VO_AANT) as VO_AANT FROM J2W_VOPX WHERE VO_PROJ = @project AND VO_SUBPROJ = @subproject AND VO_SOORT = 'U' GROUP BY VO_PROJ, VO_SUBPROJ, VO_UUR";
        Dictionary<string, object> parameters = new()
        {
            {"@project",  projectId},
            {"@subproject",  subprojectId}
        };

        //get data from ETS
        string response = ETSClient.selectQuery(query, parameters) ?? throw new Exception("Error getting uurcodes from ETS with query: " + query);

        //convert data to uurcodesETS object
        List<ProjectTaskETS> projectTasks = JsonTool.ConvertTo<List<ProjectTaskETS>>(response);

        return projectTasks;
    }
}