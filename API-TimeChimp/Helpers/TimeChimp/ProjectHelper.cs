namespace Api.Devion.Helpers.TimeChimp
{
    public static class TimeChimpProjectHelper
    {
        public static ProjectTimeChimp[] GetProjects()
        {
            var client = new BearerTokenHttpClient();

            String response = client.GetAsync("projects").Result;

            ProjectTimeChimp[] projects = JsonTool.ConvertTo<ProjectTimeChimp[]>(response);
            return projects;
        }

        public static ProjectTimeChimp[] CreateProject(ProjectTimeChimp project)
        {
            var client = new BearerTokenHttpClient();

            String response = client.PostAsync("projects", JsonTool.ConvertFrom(project)).Result;

            ProjectTimeChimp[] projects = JsonTool.ConvertTo<ProjectTimeChimp[]>(response);
            return projects;
        }

        public static ProjectTimeChimp[] UpdateProject(ProjectTimeChimp project)
        {
            var client = new BearerTokenHttpClient();

            String response = client.PutAsync("projects", JsonTool.ConvertFrom(project)).Result;

            ProjectTimeChimp[] projects = JsonTool.ConvertTo<ProjectTimeChimp[]>(response);
            return projects;
        }
    }
}
