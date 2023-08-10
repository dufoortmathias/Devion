namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpProjectUserHelper : TimeChimpHelper
{
    public TimeChimpProjectUserHelper(BearerTokenHttpClient client) : base(client)
    {
    }

    //add projectuser
    public ProjectUserTimechimp AddProjectUser(ProjectUserTimechimp projectUser)
    {
        //convert projectuser to json
        String json = JsonTool.ConvertFrom(projectUser);

        //add projectuser
        String response = TCClient.PostAsync("v1/projectusers", json).Result;

        Console.WriteLine("ProjectUser added: " + response);
        return projectUser;
    }

    //get all projectusers
    public List<ProjectUserTimechimp> GetProjectUsers()
    {
        //get data form timechimp
        String response = TCClient.GetAsync("v1/projectusers").Result;

        //convert data to projectuserTimeChimp object
        List<ProjectUserTimechimp> projectUsers = JsonTool.ConvertTo<List<ProjectUserTimechimp>>(response);
        return projectUsers;
    }

    //get projectuser by project
    public List<ProjectUserTimechimp> GetProjectUsersByProject(int projectId)
    {
        //get data form timechimp
        String response = TCClient.GetAsync($"v1/projectusers/project/{projectId}").Result;

        //convert data to projectuserTimeChimp object
        List<ProjectUserTimechimp> projectUsers = JsonTool.ConvertTo<List<ProjectUserTimechimp>>(response);
        return projectUsers;
    }

    //add all user for project
    public object AddAllProjectUserForProject(string projectNumber)
    {
        TimeChimpProjectHelper projectHelper = new(TCClient);

        //get projectId from timechimp
        Int32 projectId = projectHelper.GetProjects().Find(p => p.code.Equals(projectNumber)).id.Value;

        //get project from timechimp
        ProjectTimeChimp project = projectHelper.GetProject(projectId);

        //get projectusers from timechimp
        List<ProjectUserTimechimp> projectUsers = GetProjectUsersByProject(projectId);

        List<ProjectUserTimechimp> projectUsersAdded = new();
        foreach (EmployeeTimeChimp employee in new TimeChimpEmployeeHelper(TCClient).GetEmployees())
        {
            //check if user is not already added to project
            if (!projectUsers.Exists(e => e.userId.Equals(employee.id)))
            {
                //create projectuser
                ProjectUserTimechimp projectUser = new ProjectUserTimechimp(employee.id.Value, project.id.Value);
                ProjectUserTimechimp response = AddProjectUser(projectUser);
                projectUsersAdded.Add(projectUser);
            }
        }
        return projectUsersAdded;
    }

    //get projectuser by user
    public List<ProjectUserTimechimp> GetProjectUsersByUser(int userId)
    {
        //get data from timechimp
        String response = TCClient.GetAsync($"v1/projectusers/user/{userId}").Result;

        //convert data to projectuserTimeChimp object
        List<ProjectUserTimechimp> projectUsers = JsonTool.ConvertTo<List<ProjectUserTimechimp>>(response);
        return projectUsers;
    }

    //add all projectusers for employee
    public object AddAllProjectUserForEmployee(Int32 employeeId)
    {
        //get projectusers by user from timechimp
        List<ProjectUserTimechimp> projectUsers = GetProjectUsersByUser(employeeId);

        List<ProjectUserTimechimp> projectUsersAdded = new();
        foreach (ProjectTimeChimp project in new TimeChimpProjectHelper(TCClient).GetProjects())
        {
            //check if user is not already added to project
            if (!projectUsers.Exists(e => e.projectId.Equals(project.id)))
            {
                ProjectUserTimechimp projectUser = new ProjectUserTimechimp(employeeId, project.id.Value);
                ProjectUserTimechimp response = AddProjectUser(projectUser);
                projectUsersAdded.Add(projectUser);
            }
        }
        return projectUsersAdded;
    }
}