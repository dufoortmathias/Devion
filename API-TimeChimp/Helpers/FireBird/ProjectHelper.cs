namespace Api.Devion.Helpers.FireBird
{
    public static class FireBirdProjectHelper
    {
        public static ProjectFireBird GetProject(Int32 projectId)
        {
            FirebirdClientETS client = new();

            String query = $"SELECT * FROM PROJPX WHERE PR_NR = {projectId}";
            String json = client.selectQuery(query);
            ProjectFireBird project = JsonConvert.DeserializeObject<ProjectFireBird[]>(json).First();
            return project;
        }

        public static Int32[] GetProjectIds()
        {
            FirebirdClientETS client = new();

            String query = "SELECT PROJPX.PR_NR FROM PROJPX";
            String json = client.selectQuery(query);
            Int32[] ids = JsonConvert.DeserializeObject<ProjectFireBird[]>(json)
                .Select(project => project.PR_NR)
                .Where(x => x != null)
                .Select(x => x.Value)
                .ToArray();
            return ids;
        }
    }
}
