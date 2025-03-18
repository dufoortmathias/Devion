namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpProjectUserHelper : TimeChimpHelper
{
    public TimeChimpProjectUserHelper(WebClient client) : base(client)
    {
    }

    //add projectuser
    public ProjectUserTimechimp AddProjectUser(ProjectUserTimechimp projectUser)
    {
        //convert projectuser to json
        string json = JsonTool.ConvertFrom(projectUser);

        //check if json is not empty
        if (json == null)
        {
            throw new Exception("Error converting projectuser to json");
        }

        //add projectuser
        _ = TCClient.PostAsync("v1/projectusers", json);

        return projectUser;
    }

    //get all projectusers
    public List<ProjectUserTimechimp> GetProjectUsers()
    {
        //get data form timechimp
        string response = TCClient.GetAsync("v1/projectusers");

        //convert data to projectuserTimeChimp object
        List<ProjectUserTimechimp> projectUsers = JsonTool.ConvertTo<List<ProjectUserTimechimp>>(response);
        return projectUsers;
    }

    //get projectuser by project
    public List<ProjectUserTimechimp> GetProjectUsersByProject(int projectId)
    {
        //get data form timechimp
        string response = TCClient.GetAsync($"v1/projectusers/project/{projectId}");

        //convert data to projectuserTimeChimp object
        List<ProjectUserTimechimp> projectUsers = JsonTool.ConvertTo<List<ProjectUserTimechimp>>(response);
        return projectUsers;
    }

    //add all user for project
    public object AddAllProjectUserForProject(string projectNumber)
    {
        TimeChimpProjectHelper projectHelper = new(TCClient);

        //get projectId from timechimp
        int projectId = projectHelper.GetProjects().Find(p => p.Code != null && p.Code.Equals(projectNumber))?.Id ?? throw new Exception($"Error getting project from timechimp with code = {projectNumber}");

        //get project from timechimp
        ProjectTimeChimp project = projectHelper.GetProject(projectId) ?? throw new Exception($"Error getting project from timechimp with endpoint: v1/projects/{projectId}");

        //get projectusers from timechimp
        List<ProjectUserTimechimp> projectUsers = GetProjectUsersByProject(projectId);

        //check if projectusers is not null
        if (projectUsers == null)
        {
            throw new Exception($"Error getting projectusers from timechimp with endpoint: v1/projectusers/project/{projectId}");
        }

        List<ProjectUserTimechimp> projectUsersAdded = new();
        foreach (EmployeeTimeChimp employee in new TimeChimpEmployeeHelper(TCClient).GetEmployees())
        {
            //check if user is not already added to project
            if (!projectUsers.Exists(e => e.userId.Equals(employee.Id)))
            {
                //create projectuser
                ProjectUserTimechimp projectUser = new(employee.Id, projectId);
                ProjectUserTimechimp response = AddProjectUser(projectUser) ?? throw new Exception($"Error adding projectuser to timechimp with endpoint: v1/projectusers");
                projectUsersAdded.Add(projectUser);
            }
        }
        return projectUsersAdded;
    }

    //get projectuser by user
    public List<ProjectUserTimechimp> GetProjectUsersByUser(int userId)
    {
        //get data from timechimp
        string response = TCClient.GetAsync($"v1/projectusers/user/{userId}");

        //convert data to projectuserTimeChimp object
        List<ProjectUserTimechimp> projectUsers = JsonTool.ConvertTo<List<ProjectUserTimechimp>>(response);
        return projectUsers;
    }

    //add all projectusers for employee
    public object AddAllProjectUserForEmployee(string employeeId)
    {
        TimeChimpEmployeeHelper employeeHelper = new(TCClient);

        //get employee from timechimp
        EmployeeTimeChimp employee = employeeHelper.GetEmployeeByEmployeeNumber(employeeId);

        //check if employee is not empty
        if (employee == null)
        {
            throw new Exception($"Error getting employee from timechimp with endpoint: users/{employeeId}");
        }

        //get projectusers ny user from timechimp
        List<ProjectUserTimechimp> projectUsers = GetProjectUsersByUser(employee.Id);

        //check if projectusers is not null
        if (projectUsers == null)
        {
            throw new Exception($"Error getting projectusers from timechimp with endpoint: v1/projectusers/user/{employeeId}");
        }

        List<ProjectUserTimechimp> projectUsersAdded = new();
        foreach (ProjectTimeChimp project in new TimeChimpProjectHelper(TCClient).GetProjects())
        {
            //check if user is not already added to project
            if (!projectUsers.Exists(e => e.projectId.Equals(project.Id)))
            {
                ProjectUserTimechimp projectUser = new(employee.Id, project.Id);
                ProjectUserTimechimp response = AddProjectUser(projectUser) ?? throw new Exception($"Error adding projectuser to timechimp with endpoint: v1/projectusers");
                projectUsersAdded.Add(projectUser);
            }
        }
        return projectUsersAdded;
    }
}