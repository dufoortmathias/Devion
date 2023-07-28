namespace Api.Devion.Helpers.TimeChimp
{
    public static class TimeChimpProjectHelper
    {
        public static Boolean ProjectExists(Int32 projectId)
        {
            var client = new BearerTokenHttpClient();

            String response = client.GetAsync($"projects/{projectId}").Result;
            return response != null;
        }

        public static ProjectTimeChimp[] GetProjects()
        {
            var client = new BearerTokenHttpClient();

            String response = client.GetAsync("projects").Result;

            ProjectTimeChimp[] projects = JsonTool.ConvertTo<ProjectTimeChimp[]>(response);
            return projects;
        }

        public static ProjectTimeChimp CreateProject(ProjectTimeChimp project)
        {
            var client = new BearerTokenHttpClient();

            String json = JsonTool.ConvertFrom(project);
            String response = client.PostAsync("projects", json).Result;

            ProjectTimeChimp projectResponse = JsonTool.ConvertTo<ProjectTimeChimp>(response);
            return projectResponse;
        }

        public static ProjectTimeChimp UpdateProject(ProjectTimeChimp project)
        {
            var client = new BearerTokenHttpClient();

            String response = client.PutAsync("projects", JsonTool.ConvertFrom(project)).Result;

            ProjectTimeChimp projectResponse = JsonTool.ConvertTo<ProjectTimeChimp>(response);
            return projectResponse;
        }
    }
}
