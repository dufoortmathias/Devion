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

        //check if json is not empty
        if (json == null)
        {
            throw new Exception("Error converting projectuser to json");
        }

        //add projectuser
        var response = TCClient.PostAsync("v1/projectusers", json);

        //check if response is succesfull
        if (!response.IsCompletedSuccessfully)
        {
            throw new Exception("Error adding projectuser to timechimp with endpoint: v1/projectusers");
        }

        return projectUser;
    }

    //get all projectusers
    public List<ProjectUserTimechimp> GetProjectUsers()
    {
        //get data form timechimp
        var response = TCClient.GetAsync("v1/projectusers");

        //check if response is succesfull
        if (!response.IsCompletedSuccessfully)
        {
            throw new Exception("Error getting projectusers from timechimp with endpoint: v1/projectusers");
        }

        //convert data to projectuserTimeChimp object
        List<ProjectUserTimechimp> projectUsers = JsonTool.ConvertTo<List<ProjectUserTimechimp>>(response.Result);
        return projectUsers;
    }

    //get projectuser by project
    public List<ProjectUserTimechimp> GetProjectUsersByProject(int projectId)
    {
        //get data form timechimp
        var response = TCClient.GetAsync($"v1/projectusers/project/{projectId}");

        //check if response is succesfull
        if (!response.IsCompletedSuccessfully)
        {
            throw new Exception($"Error getting projectusers from timechimp with endpoint: v1/projectusers/project/{projectId}");
        }

        //convert data to projectuserTimeChimp object
        List<ProjectUserTimechimp> projectUsers = JsonTool.ConvertTo<List<ProjectUserTimechimp>>(response.Result);
        return projectUsers;
    }

    //add all user for project
    public object AddAllProjectUserForProject(string projectNumber)
    {
        TimeChimpProjectHelper projectHelper = new(TCClient);

        //get projectId from timechimp
        Int32 projectId = projectHelper.GetProjects().Find(p => p.code.Equals(projectNumber)).id.Value;

        //check if projectID is not null
        if (projectId == null)
        {
            throw new Exception($"Error getting project from timechimp with endpoint: v1/projects/{projectId}");
        }

        //get project from timechimp
        ProjectTimeChimp project = projectHelper.GetProject(projectId);

        //check if project is not null
        if (project == null)
        {
            throw new Exception($"Error getting project from timechimp with endpoint: v1/projects/{projectId}");
        }

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
            if (!projectUsers.Exists(e => e.userId.Equals(employee.id)))
            {
                //create projectuser
                ProjectUserTimechimp projectUser = new ProjectUserTimechimp(employee.id.Value, project.id.Value);
                ProjectUserTimechimp response = AddProjectUser(projectUser);

                //check if projectuser is not null
                if (response == null)
                {
                    throw new Exception($"Error adding projectuser to timechimp with endpoint: v1/projectusers");
                }

                projectUsersAdded.Add(projectUser);
            }
        }
        return projectUsersAdded;
    }

    //get projectuser by user
    public List<ProjectUserTimechimp> GetProjectUsersByUser(int userId)
    {
        //get data from timechimp
        var response = TCClient.GetAsync($"v1/projectusers/user/{userId}");

        //check if response is succesfull
        if (!response.IsCompletedSuccessfully)
        {
            throw new Exception($"Error getting projectusers from timechimp with endpoint: v1/projectusers/user/{userId}");
        }

        //convert data to projectuserTimeChimp object
        List<ProjectUserTimechimp> projectUsers = JsonTool.ConvertTo<List<ProjectUserTimechimp>>(response.Result);
        return projectUsers;
    }

    //add all projectusers for employee
    public object AddAllProjectUserForEmployee(Int32 employeeId)
    {
        TimeChimpEmployeeHelper employeeHelper = new(TCClient);

        //get employeeId from timechimp
        Int32 employeeId = employeeHelper.GetEmployees().ToList().Find(e => e.employeeNumber.Equals(employeeNumber)).id.Value;

        //check if employeeid is not null
        if (employeeId == null)
        {
            throw new Exception($"Error getting employee from timechimp with endpoint: v1/users/{employeeId}");
        }

        //get employee from timechimp
        EmployeeTimeChimp employee = employeeHelper.GetEmployee(employeeId);

        //check if employee is not empty
        if (employee == null)
        {
            throw new Exception($"Error getting employee from timechimp with endpoint: v1/users/{employeeId}");
        }

        //get projectusers ny user from timechimp
        List<ProjectUserTimechimp> projectUsers = GetProjectUsersByUser(employeeId);

        //check if projectusers is not null
        if (projectUsers == null)
        {
            throw new Exception($"Error getting projectusers from timechimp with endpoint: v1/projectusers/user/{employeeId}");
        }

        List<ProjectUserTimechimp> projectUsersAdded = new();
        foreach (ProjectTimeChimp project in new TimeChimpProjectHelper(TCClient).GetProjects())
        {
            //check if user is not already added to project
            if (!projectUsers.Exists(e => e.projectId.Equals(project.id)))
            {
                ProjectUserTimechimp projectUser = new ProjectUserTimechimp(employeeId, project.id.Value);
                ProjectUserTimechimp response = AddProjectUser(projectUser);

                //check if projectuser is not null
                if (response == null)
                {
                    throw new Exception($"Error adding projectuser to timechimp with endpoint: v1/projectusers");
                }

                projectUsersAdded.Add(projectUser);
            }
        }
        return projectUsersAdded;
    }
}