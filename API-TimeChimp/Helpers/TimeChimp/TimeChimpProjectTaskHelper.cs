namespace Api.Devion.Helpers.TimeChimp
{
    public class TimeChimpProjectTaskHelper : TimeChimpHelper
    {
        public TimeChimpProjectTaskHelper(WebClient client) : base(client)
        {
        }

        public TaskTimechimp? FindProjectTask(int taskId, int subprojectId)
        {
            string response = TCClient.GetAsync($"v1/projecttasks/project/{subprojectId}");

            List<TaskTimechimp> projectTasks = JsonTool.ConvertTo<List<TaskTimechimp>>(response);

            return projectTasks.Find(pt => pt.Id == taskId);
        }

        public void CreateOrUpdateProjectTask(int taskId, int subprojectId, double aantal)
        {
            TaskTimechimp projectTask = FindProjectTask(taskId, subprojectId) ?? new()
            {
                Id = taskId,
                ProjectId = subprojectId,
                BudgetHours = aantal
            };

            if (projectTask.Id != null)
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
