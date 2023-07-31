namespace Api.Devion.Helpers.ETS
{
    public static class ETSProjectHelper
    {
        public static ProjectETS GetProject(String projectId)
        {
            FirebirdClientETS client = new();

            string query = $"SELECT * FROM PROJPX WHERE PR_NR = {projectId}";
            string json = client.selectQuery(query);
            ProjectETS project = JsonTool.ConvertTo<ProjectETS[]>(json).First();
            return project;
        }

        public static List<SubprojectETS> GetSubprojects(String projectId)
        {
            FirebirdClientETS client = new();

            string query = $"SELECT * FROM SUBPROJ WHERE SU_NR = {projectId}";
            string json = client.selectQuery(query);
            List<SubprojectETS> subprojects = JsonTool.ConvertTo<List<SubprojectETS>>(json);
            return subprojects;
        }

        public static String[] GetProjectIdsChangedAfter(DateTime date)
        {
            FirebirdClientETS client = new();

            string query = $"SELECT PROJPX.PR_NR FROM PROJPX WHERE DATE_CHANGED BETWEEN '{date.ToString("MM/dd/yyyy HH:mm")}' AND '{DateTime.Now.ToString("MM/dd/yyyy HH:mm")}'";
            string json = client.selectQuery(query);
            String[] ids = JsonTool.ConvertTo<ProjectETS[]>(json)
                .Select(project => project.PR_NR)
                .ToArray();
            return ids;
        }
    }
}
