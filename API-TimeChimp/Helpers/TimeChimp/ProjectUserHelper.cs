namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpProjectUserHelper
{
    public static ProjectUserTimechimp AddProjectUser(ProjectUserTimechimp projectUser)
    {
        var client = new BearerTokenHttpClient();
        String json = JsonTool.ConvertFrom(projectUser);
        String response = client.PostAsync("projectusers", json).Result;

        Console.WriteLine("ProjectUser added: " + response);
        return projectUser;
    }

    public static List<ProjectUserTimechimp> GetProjectUsers()
    {
        var client = new BearerTokenHttpClient();

        String response = client.GetAsync("projectusers").Result;
        List<ProjectUserTimechimp> projectUsers = JsonTool.ConvertTo<List<ProjectUserTimechimp>>(response);
        return projectUsers;
    }

    public static List<ProjectUserTimechimp> GetProjectUsersByProject(int projectId)
    {
        var client = new BearerTokenHttpClient();

        String response = client.GetAsync($"projectusers/project/{projectId}").Result;
        List<ProjectUserTimechimp> projectUsers = JsonTool.ConvertTo<List<ProjectUserTimechimp>>(response);
        return projectUsers;
    }

    public static object AddProjectUserProject(string projectNumber)
    {
        Int32 projectId = TimeChimpProjectHelper.GetProjects().Find(p => p.code.Equals(projectNumber)).id.Value;
        ProjectTimeChimp project = TimeChimpProjectHelper.GetProject(projectId);

        List<ProjectUserTimechimp> projectUsers = GetProjectUsersByProject(projectId);

        List<ProjectUserTimechimp> projectUsersAdded = new();
        foreach (EmployeeTimeChimp employee in TimeChimpEmployeeHelper.GetEmployees())
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

    public static List<ProjectUserTimechimp> GetProjectUsersByUser(int userId)
    {
        var client = new BearerTokenHttpClient();

        String response = client.GetAsync($"projectusers/user/{userId}").Result;
        List<ProjectUserTimechimp> projectUsers = JsonTool.ConvertTo<List<ProjectUserTimechimp>>(response);
        return projectUsers;
    }

    public static object AddProjectUserEmployee(string employeeNumber)
    {
        Int32 employeeId = TimeChimpEmployeeHelper.GetEmployees().ToList().Find(e => e.employeeNumber.Equals(employeeNumber)).id.Value;
        EmployeeTimeChimp employee = TimeChimpEmployeeHelper.GetEmployee(employeeId);

        List<ProjectUserTimechimp> projectUsers = GetProjectUsersByUser(employeeId);

        List<ProjectUserTimechimp> projectUsersAdded = new();
        var projects = TimeChimpProjectHelper.GetProjects();
        Console.WriteLine(projects.Count);
        foreach (ProjectTimeChimp project in projects)
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