using api_Devion.Models;

namespace API_TimeChimp.Helpers.TimeChimp
{
    public static class TimeChimpProjectHelper
    {
        public static ProjectTimeChimp[] GetProjects()
        {
            var client = new BearerTokenHttpClient();

            String response = client.GetAsync("projects").Result;

            ProjectTimeChimp[] projects = JsonConvert.DeserializeObject<ProjectTimeChimp[]>(response);
            return projects;
        }

        public static ProjectTimeChimp[] CreateProject(ProjectTimeChimp project)
        {
            var client = new BearerTokenHttpClient();

            String response = client.PostAsync("projects", JsonConvert.SerializeObject(project)).Result;

            ProjectTimeChimp[] projects = JsonConvert.DeserializeObject<ProjectTimeChimp[]>(response);
            return projects;
        }

        public static ProjectTimeChimp[] UpdateProject(ProjectTimeChimp project)
        {
            var client = new BearerTokenHttpClient();

            String response = client.PutAsync("projects", JsonConvert.SerializeObject(project)).Result;

            ProjectTimeChimp[] projects = JsonConvert.DeserializeObject<ProjectTimeChimp[]>(response);
            return projects;
        }
    }
}
