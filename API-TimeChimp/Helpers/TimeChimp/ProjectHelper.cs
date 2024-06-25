namespace Api.Devion.Helpers.TimeChimp
{
    public class TimeChimpProjectHelper: TimeChimpHelper
    {
        public TimeChimpProjectHelper(WebClient client) : base(client)
        {
        }

        //check if project exists
        public ProjectTimeChimp? FindProject(string projectId)
        {
            ProjectTimeChimp? project = GetProjects().Find(project => project.Code != null && project.Code.Equals(projectId));
            if (project != null)
            {
                project = GetProject(project.Id);
            }
            return project;
        }

        //get project by id
        public ProjectTimeChimp GetProject(int projectId)
        {
            //get data form timechimp
            string response = TCClient.GetAsync($"projects/{projectId}?$expand=projectTasks, projectUsers, subprojects");

            //convert data to projectTimeChimp object
            ProjectTimeChimp project = JsonTool.ConvertTo<ResponseTCProject>(response).Result[0];
            return project;
        }

        //get all projects
        public List<ProjectTimeChimp> GetProjects()
        {
            //get data from timechimp
            string response = TCClient.GetAsync("projects?$expand=subprojects");

            //convert data to projectTimeChimp object
            List<ProjectTimeChimp> projects = JsonTool.ConvertTo<ResponseTCProject>(response).Result.ToList();
            return projects;
        }

        //create project
        public ProjectTimeChimp CreateProject(ProjectTimeChimp project)
        {
            //convert project to json
			string json = JsonTool.ConvertFrom(project) ?? throw new Exception("Error converting project to json");

            //add project to timechimp
            string response = TCClient.PostAsync("projects", json);

            //convert response to projectTimeChimp object
            ProjectTimeChimp projectResponse = JsonTool.ConvertTo<ResponseTCProject>(response).Result[0];
            return projectResponse;
        }

        //update project
        public ProjectTimeChimp UpdateProject(ProjectTimeChimp projectUpdate)
        {
            //get project from timechimp
            ProjectTimeChimp project = GetProject(projectUpdate.Id) ?? throw new Exception("Project does not exist in timechimp");

            //update some fields
            project.Name = projectUpdate.Name;
            project.EndDate = projectUpdate.EndDate;
            project.StartDate = projectUpdate.StartDate;
            project.Active = projectUpdate.Active;
            project.Budget = new();
            project.Budget.Method = projectUpdate.Budget.Method;
            project.Budget.Hours = projectUpdate.Budget.Hours;
            project.Customer = new();
            project.Customer.Id = projectUpdate.Customer.Id;
            //send data to timechimp
            string response = TCClient.PutAsync($"projects/{project.Id}", JsonTool.ConvertFrom(project));

            Console.WriteLine(response);                                                    
            //convert response to projectTimeChimp object
            ProjectTimeChimp projectResponse = JsonTool.ConvertTo<ResponseTCProject>(response).Result[0];
            return projectResponse;
        }

        public ResponseTCProject GetProjectTimeChimpByETSId(string projectId)
        {
            //get data from timechimp
            string response = TCClient.GetAsync($"projects?$filter=code eq '{projectId}'&$count=true");

            //convert data to projectTimeChimp object
            ResponseTCProject project = JsonTool.ConvertTo<ResponseTCProject>(response);
            return project;
        }
    }
}
