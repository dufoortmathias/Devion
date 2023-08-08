namespace Api.Devion.Helpers.TimeChimp
{
    public class TimeChimpProjectHelper : TimeChimpHelper
    {
        public TimeChimpProjectHelper(BearerTokenHttpClient client) : base(client)
        {
        }

        public Boolean ProjectExists(String projectId)
        {
            return GetProjects().Any(project => project.code != null && project.code.Equals(projectId));
        }

        public ProjectTimeChimp GetProject(Int32 projectId)
        {
            String response = TCClient.GetAsync($"v1/projects/{projectId}").Result;
            Console.WriteLine(response);
            ProjectTimeChimp project = JsonTool.ConvertTo<ProjectTimeChimp>(response);
            return project;
        }

        public List<ProjectTimeChimp> GetProjects()
        {
            String response = TCClient.GetAsync("v2/projects").Result;

            List<ProjectTimeChimp> projects = JsonTool.ConvertTo<List<ProjectTimeChimp>>(response);
            return projects;
        }

        public ProjectTimeChimp CreateProject(ProjectTimeChimp project)
        {
            String json = JsonTool.ConvertFrom(project);
            String response = TCClient.PostAsync("v1/projects", json).Result;

            ProjectTimeChimp projectResponse = JsonTool.ConvertTo<ProjectTimeChimp>(response);
            return projectResponse;
        }

        public ProjectTimeChimp UpdateProject(ProjectTimeChimp projectUpdate)
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

            String response = TCClient.PutAsync("v1/projects", JsonTool.ConvertFrom(project)).Result;
            ProjectTimeChimp projectResponse = JsonTool.ConvertTo<ProjectTimeChimp>(response);
            return projectResponse;
        }

        public int GetProjectId(int projectId)
        {
            return Int32.Parse(GetProjects().ToList().Find(p => p.id.Equals(projectId)).code);
        }
    }
}
