namespace Api.Devion.Helpers.TimeChimp
{
    public static class TimeChimpProjectHelper
    {
        public static Boolean ProjectExists(Int32 projectId)
        {
            var client = new BearerTokenHttpClient();

            String response = client.GetAsync($"projects/{projectId}").Result;

            ProjectTimeChimp? project = JsonConvert.DeserializeObject<ProjectTimeChimp>(response);
            return project != null;
        }

        public static ProjectTimeChimp[] GetProjects()
        {
            var client = new BearerTokenHttpClient();

            String response = client.GetAsync("projects").Result;

            ProjectTimeChimp[] projects = JsonConvert.DeserializeObject<ProjectTimeChimp[]>(response);
            return projects;
        }

        public static ProjectTimeChimp CreateProject(ProjectTimeChimp project)
        {
            var client = new BearerTokenHttpClient();

            String json = JsonConvert.SerializeObject(project, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            String response = client.PostAsync("projects", json).Result;

            ProjectTimeChimp projectResponse = JsonConvert.DeserializeObject<ProjectTimeChimp>(response);
            return projectResponse;
        }

        public static ProjectTimeChimp UpdateProject(ProjectTimeChimp project)
        {
            var client = new BearerTokenHttpClient();

            String response = client.PutAsync("projects", JsonConvert.SerializeObject(project)).Result;

            ProjectTimeChimp projectResponse = JsonConvert.DeserializeObject<ProjectTimeChimp>(response);
            return projectResponse;
        }
    }
}
