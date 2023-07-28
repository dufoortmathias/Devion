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

        public static String[] GetProjectIds()
        {
            FirebirdClientETS client = new();

            string query = "SELECT PROJPX.PR_NR FROM PROJPX";
            string json = client.selectQuery(query);
            String[] ids = JsonTool.ConvertTo<ProjectETS[]>(json)
                .Select(project => project.PR_NR)
                .ToArray();
            return ids;
        }
    }
}
