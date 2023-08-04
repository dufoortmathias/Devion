namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpProjectUserHelper
{
    public static ProjectUserTimechimp AddProjectUser(ProjectUserTimechimp projectUser)
    {
        var client = new BearerTokenHttpClient();
        String json = JsonTool.ConvertFrom(projectUser);
        Console.WriteLine("Adding projectUser: " + json);
        String response = client.PutAsync("projectusers", json).Result;

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

        String response = client.GetAsync($"projectusers/project/{projectId.ToString()}").Result;
        Console.WriteLine("ProjectUsers: " + response);
        List<ProjectUserTimechimp> projectUsers = JsonTool.ConvertTo<List<ProjectUserTimechimp>>(response);
        return projectUsers;
    }

    public static object AddProjectUserProject(string projectId)
    {
        projectId = TimeChimpProjectHelper.GetProjects().Find(p => p.code.Equals(projectId)).id.ToString();
        var project = TimeChimpProjectHelper.GetProject(Int32.Parse(projectId));
        var projectId2 = TimeChimpProjectHelper.GetProjects().Find(p => p.code.Equals("00000020001")).id.ToString();
        var project2 = TimeChimpProjectHelper.GetProject(Int32.Parse(projectId2));

        var projectUser2 = GetProjectUsersByProject(project2.id.Value);
        var projectUser = new List<ProjectUserTimechimp>();

        foreach (var user in projectUser2)
        {
            user.projectId = project.id;
            projectUser.Add(user);
        }

        foreach (var user in projectUser)
        {
            var response = AddProjectUser(user);
            Console.WriteLine("ProjectUser added: " + JsonTool.ConvertFromWithNullValues(response));
        }

        return projectUser;
    }
}