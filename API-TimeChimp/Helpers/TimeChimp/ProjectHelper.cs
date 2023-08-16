namespace Api.Devion.Helpers.TimeChimp
{
    public class TimeChimpProjectHelper : TimeChimpHelper
    {
        public TimeChimpProjectHelper(BearerTokenHttpClient client) : base(client)
        {
        }

        //check if project exists
        public ProjectTimeChimp? FindProject(String projectId)
        {
            return GetProjects().Find(project => project.code != null && project.code.Equals(projectId));
        }

        //get project by id
        public ProjectTimeChimp GetProject(Int32 projectId)
        {
            //get data form timechimp
            String response = TCClient.GetAsync($"v1/projects/{projectId}");

            //convert data to projectTimeChimp object
            ProjectTimeChimp project = JsonTool.ConvertTo<ProjectTimeChimp>(response);
            return project;
        }

        //get all projects
        public List<ProjectTimeChimp> GetProjects()
        {
            //get data from timechimp
            String response = TCClient.GetAsync("v2/projects");

            //convert data to projectTimeChimp object
            List<ProjectTimeChimp> projects = JsonTool.ConvertTo<List<ProjectTimeChimp>>(response);
            return projects;
        }

        //create project
        public ProjectTimeChimp CreateProject(ProjectTimeChimp project)
        {
            //convert project to json
            String json = JsonTool.ConvertFrom(project);

            //check if json is not empty
            if (json == null)
            {
                throw new Exception("Error converting project to json");
            }

            //add project to timechimp
            String response = TCClient.PostAsync("v1/projects", json);

            //convert response to projectTimeChimp object
            ProjectTimeChimp projectResponse = JsonTool.ConvertTo<ProjectTimeChimp>(response);
            return projectResponse;
        }

        //update project
        public ProjectTimeChimp UpdateProject(ProjectTimeChimp projectUpdate)
        {
            //get project from timechimp
            ProjectTimeChimp project = GetProject(projectUpdate.id.Value);

            //check if project exists
            if (project == null)
            {
                throw new Exception("Project does not exist in timechimp");
            }

            //update some fields
            project.name = projectUpdate.name;
            project.endDate = projectUpdate.endDate;
            project.startDate = projectUpdate.startDate;
            project.active = projectUpdate.active;

            //send data to timechimp
            String response = TCClient.PutAsync("v1/projects", JsonTool.ConvertFrom(project));

            //convert response to projectTimeChimp object
            ProjectTimeChimp projectResponse = JsonTool.ConvertTo<ProjectTimeChimp>(response);
            return projectResponse;
        }

        //get project id
        public int GetProjectId(int projectId)
        {
            return Int32.Parse(GetProjects().ToList().Find(p => p.id.Equals(projectId)).code);
        }
    }
}
