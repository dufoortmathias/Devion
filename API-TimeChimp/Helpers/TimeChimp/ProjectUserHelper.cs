namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpProjectUserHelper : TimeChimpHelper
{
    public TimeChimpProjectUserHelper(BearerTokenHttpClient client) : base(client)
    {
    }

    public ProjectUserTimechimp AddProjectUser(ProjectUserTimechimp projectUser)
    {
        String json = JsonTool.ConvertFrom(projectUser);
        String response = TCClient.PostAsync("v1/projectusers", json).Result;

        Console.WriteLine("ProjectUser added: " + response);
        return projectUser;
    }

    public List<ProjectUserTimechimp> GetProjectUsers()
    {
        String response = TCClient.GetAsync("v1/projectusers").Result;
        List<ProjectUserTimechimp> projectUsers = JsonTool.ConvertTo<List<ProjectUserTimechimp>>(response);
        return projectUsers;
    }

    public List<ProjectUserTimechimp> GetProjectUsersByProject(int projectId)
    {
        String response = TCClient.GetAsync($"v1/projectusers/project/{projectId}").Result;
        List<ProjectUserTimechimp> projectUsers = JsonTool.ConvertTo<List<ProjectUserTimechimp>>(response);
        return projectUsers;
    }

    public object AddProjectUserProject(string projectNumber)
    {
        TimeChimpProjectHelper projectHelper = new(TCClient);
        Int32 projectId = projectHelper.GetProjects().Find(p => p.code.Equals(projectNumber)).id.Value;
        ProjectTimeChimp project = projectHelper.GetProject(projectId);

        List<ProjectUserTimechimp> projectUsers = GetProjectUsersByProject(projectId);

        List<ProjectUserTimechimp> projectUsersAdded = new();
        foreach (EmployeeTimeChimp employee in new TimeChimpEmployeeHelper(TCClient).GetEmployees())
        {
            if (!projectUsers.Exists(e => e.userId.Equals(employee.id)))
            {
                ProjectUserTimechimp projectUser = new ProjectUserTimechimp(employee, project);
                ProjectUserTimechimp response = AddProjectUser(projectUser);
                projectUsersAdded.Add(projectUser);
            }
        }
        return projectUsersAdded;
    }

    public List<ProjectUserTimechimp> GetProjectUsersByUser(int userId)
    {
        String response = TCClient.GetAsync($"v1/projectusers/user/{userId}").Result;
        List<ProjectUserTimechimp> projectUsers = JsonTool.ConvertTo<List<ProjectUserTimechimp>>(response);
        return projectUsers;
    }

    public object AddProjectUserEmployee(string employeeNumber)
    {
        TimeChimpEmployeeHelper employeeHelper = new(TCClient);
        Int32 employeeId = employeeHelper.GetEmployees().ToList().Find(e => e.employeeNumber.Equals(employeeNumber)).id.Value;
        EmployeeTimeChimp employee = employeeHelper.GetEmployee(employeeId);

        List<ProjectUserTimechimp> projectUsers = GetProjectUsersByUser(employeeId);

        List<ProjectUserTimechimp> projectUsersAdded = new();
        foreach (ProjectTimeChimp project in new TimeChimpProjectHelper(TCClient).GetProjects())
        {
            if (!projectUsers.Exists(e => e.projectId.Equals(project.id)))
            {
                ProjectUserTimechimp projectUser = new ProjectUserTimechimp(employee, project);
                ProjectUserTimechimp response = AddProjectUser(projectUser);
                projectUsersAdded.Add(projectUser);
            }
        }
        return projectUsersAdded;
    }
}