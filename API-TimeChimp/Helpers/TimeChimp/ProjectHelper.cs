namespace Api.Devion.Helpers.TimeChimp
{
    public class TimeChimpProjectHelper : TimeChimpHelper
    {
        public TimeChimpProjectHelper(BearerTokenHttpClient client) : base(client)
        {
        }

        //check if project exists
        public Boolean ProjectExists(String projectId)
        {
            return GetProjects().Any(project => project.code != null && project.code.Equals(projectId));
        }

        //get project by id
        public ProjectTimeChimp GetProject(Int32 projectId)
        {
            //get data form timechimp
            var response = TCClient.GetAsync($"v1/projects/{projectId}");

            //check if response is succesfull
            if (!response.IsCompletedSuccessfully)
            {
                throw new Exception($"Error getting project from timechimp with endpoint: v1/projects/{projectId}");
            }

            //convert data to projectTimeChimp object
            ProjectTimeChimp project = JsonTool.ConvertTo<ProjectTimeChimp>(response.Result);
            return project;
        }

        //get all projects
        public List<ProjectTimeChimp> GetProjects()
        {
            //get data from timechimp
            var response = TCClient.GetAsync("v2/projects");

            //check if response is succesfull
            if (!response.IsCompletedSuccessfully)
            {
                throw new Exception("Error getting all projects from timechimp with endpoint: v2/projects");
            }

            //convert data to projectTimeChimp object
            List<ProjectTimeChimp> projects = JsonTool.ConvertTo<List<ProjectTimeChimp>>(response.Result);
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
            var response = TCClient.PostAsync("v1/projects", json);

            //check if response is succesfull
            if (!response.IsCompletedSuccessfully)
            {
                throw new Exception("Error creating project in timechimp with endpoint: v1/projects");
            }

            //convert response to projectTimeChimp object
            ProjectTimeChimp projectResponse = JsonTool.ConvertTo<ProjectTimeChimp>(response.Result);
            return projectResponse;
        }

        //update project
        public ProjectTimeChimp UpdateProject(ProjectTimeChimp projectUpdate)
        {
            //check if projectid is null
            if (projectUpdate.id == null)
            {
                projectUpdate.id = GetProjects().ToList().Find(p => p.code.Equals(projectUpdate.code)).id;
            }

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
            var response = TCClient.PutAsync("v1/projects", JsonTool.ConvertFrom(project));

            //check if response is succesfull
            if (!response.IsCompletedSuccessfully)
            {
                throw new Exception("Error updating project in timechimp with endpoint: v1/projects");
            }

            //convert response to projectTimeChimp object
            ProjectTimeChimp projectResponse = JsonTool.ConvertTo<ProjectTimeChimp>(response.Result);
            return projectResponse;
        }

        //get project id
        public int GetProjectId(int projectId)
        {
            return Int32.Parse(GetProjects().ToList().Find(p => p.id.Equals(projectId)).code);
        }
    }
}
