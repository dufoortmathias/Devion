namespace Api.Devion.Helpers.ETS
{
    public class ETSProjectHelper : ETSHelper
    {
        public ETSProjectHelper(FirebirdClientETS FBClient) : base(FBClient)
        {
        }

        public ProjectETS GetProject(String projectId)
        {
            string query = $"SELECT * FROM PROJPX WHERE PR_NR = {projectId}";
            string json = ETSClient.selectQuery(query);
            ProjectETS project = JsonTool.ConvertTo<ProjectETS[]>(json).First();
            return project;
        }

        public List<SubprojectETS> GetSubprojects(String projectId)
        {
            string query = $"SELECT * FROM SUBPROJ WHERE VOLNR like '{projectId}%'";
            string json = ETSClient.selectQuery(query);
            List<SubprojectETS> subprojects = JsonTool.ConvertTo<List<SubprojectETS>>(json);
            return subprojects;
        }

        public String[] GetProjectIdsChangedAfter(DateTime date)
        {
            string query = $"SELECT PROJPX.PR_NR FROM PROJPX WHERE DATE_CHANGED BETWEEN '{date.ToString("MM/dd/yyyy HH:mm")}' AND '{DateTime.Now.ToString("MM/dd/yyyy HH:mm")}'";
            string json = ETSClient.selectQuery(query);
            String[] ids = JsonTool.ConvertTo<ProjectETS[]>(json)
                .Select(project => project.PR_NR)
                .ToArray();
            return ids;
        }
    }
}
