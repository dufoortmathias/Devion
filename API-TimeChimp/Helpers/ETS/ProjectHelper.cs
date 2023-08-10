namespace Api.Devion.Helpers.ETS
{
    public class ETSProjectHelper : ETSHelper
    {
        public ETSProjectHelper(FirebirdClientETS FBClient) : base(FBClient)
        {
        }

        // get project by projectId
        public ProjectETS GetProject(String projectId)
        {
            //create query
            string query = $"SELECT * FROM PROJPX WHERE PR_NR = {projectId}";

            //get data from ETS
            string json = ETSClient.selectQuery(query);

            //convert data to projectETS object
            ProjectETS project = JsonTool.ConvertTo<ProjectETS[]>(json).First();
            return project;
        }

        //get all subprojects from a project
        public List<SubprojectETS> GetSubprojects(String projectId)
        {
            //create query
            string query = $"SELECT * FROM SUBPROJ WHERE VOLNR like '{projectId}%'";

            //get data from ETS
            string json = ETSClient.selectQuery(query);

            //convert data to subprojectETS object
            List<SubprojectETS> subprojects = JsonTool.ConvertTo<List<SubprojectETS>>(json);
            return subprojects;
        }

        //get all projectids that are changed after the given date
        public String[] GetProjectIdsChangedAfter(DateTime date)
        {
            //create query
            string query = $"SELECT PROJPX.PR_NR FROM PROJPX WHERE DATE_CHANGED BETWEEN '{date.ToString("MM/dd/yyyy HH:mm")}' AND '{DateTime.Now.ToString("MM/dd/yyyy HH:mm")}'";

            //get data from ETS
            string json = ETSClient.selectQuery(query);

            //get all ids from the json
            String[] ids = JsonTool.ConvertTo<ProjectETS[]>(json)
                .Select(project => project.PR_NR)
                .ToArray();
            return ids;
        }
    }
}
