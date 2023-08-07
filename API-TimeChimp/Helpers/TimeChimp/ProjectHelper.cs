using System.Runtime.InteropServices;

namespace Api.Devion.Helpers.TimeChimp
{
    public static class TimeChimpProjectHelper
    {
        public static Boolean ProjectExists(String projectId)
        {
            return GetProjects().Any(project => project.code != null && project.code.Equals(projectId));
        }

        public static ProjectTimeChimp GetProject(Int32 projectId)
        {
            var client = new BearerTokenHttpClient();

            String response = client.GetAsync($"projects/{projectId}").Result;
            Console.WriteLine(response);
            ProjectTimeChimp project = JsonTool.ConvertTo<ProjectTimeChimp>(response);
            return project;
        }

        public static List<ProjectTimeChimp> GetProjects()
        {
            var client = new BearerTokenHttpClient("https://api.timechimp.com/v2/");

            String response = client.GetAsync("projects").Result;

            List<ProjectTimeChimp> projects = JsonTool.ConvertTo<List<ProjectTimeChimp>>(response);
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

        public static ProjectTimeChimp UpdateProject(ProjectTimeChimp projectUpdate)
        {
            if (projectUpdate.id == null)
            {
                projectUpdate.id = GetProjects().ToList().Find(p => p.code.Equals(projectUpdate.code)).id;
            }

            ProjectTimeChimp project = GetProject(projectUpdate.id.Value);

            project.name = projectUpdate.name;
            project.endDate = projectUpdate.endDate;
            project.startDate = projectUpdate.startDate;
            project.active = projectUpdate.active;

            var client = new BearerTokenHttpClient();
            String response = client.PutAsync("projects", JsonTool.ConvertFrom(project)).Result;
            ProjectTimeChimp projectResponse = JsonTool.ConvertTo<ProjectTimeChimp>(response);
            return projectResponse;
        }

        public static int GetProjectId(int projectId)
        {
            return Int32.Parse(GetProjects().ToList().Find(p => p.id.Equals(projectId)).code);
        }
    }
}
