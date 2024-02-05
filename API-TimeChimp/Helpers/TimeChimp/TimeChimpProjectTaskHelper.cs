namespace Api.Devion.Helpers.TimeChimp
{
    public class TimeChimpProjectTaskHelper : TimeChimpHelper
    {
        public TimeChimpProjectTaskHelper(WebClient client) : base(client)
        {
        }

        public ProjectTaskTimechimp? FindProjectTask(int taskId, int subprojectId)
        {
            string response = TCClient.GetAsync($"v1/projecttasks/project/{subprojectId}");

            List<ProjectTaskTimechimp> projectTasks = JsonTool.ConvertTo<List<ProjectTaskTimechimp>>(response);

            return projectTasks.Find(pt => pt.taskId == taskId);
        }

        public void CreateOrUpdateProjectTask(int taskId, int subprojectId, double aantal)
        {
            ProjectTaskTimechimp projectTask = FindProjectTask(taskId, subprojectId) ?? new()
            {
                taskId = taskId,
                projectId = subprojectId,
                budgetHours = aantal
            };

            if (projectTask.id != null)
            {
                TCClient.PutAsync("v1/projecttasks", JsonTool.ConvertFrom(projectTask));
            }
            else
            {
                TCClient.PostAsync("v1/projecttasks", JsonTool.ConvertFrom(projectTask));
            }

        }
    }
}
