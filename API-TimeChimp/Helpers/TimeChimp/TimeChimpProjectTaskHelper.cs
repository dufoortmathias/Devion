using Api.Devion.Models;
using System.Threading.Tasks;

namespace Api.Devion.Helpers.TimeChimp
{
    public class TimeChimpProjectTaskHelper : TimeChimpHelper
    {
        public TimeChimpProjectTaskHelper(WebClient client) : base(client)
        {
        }

        public void CreateOrUpdateProjectTask(int taskId, int subprojectId, double aantal)
        {
            ProjectTaskTimechimp projectTask = new()
            {
                taskId = taskId,
                projectId = subprojectId,
                budgetHours = aantal
            };

            TCClient.PutAsync("v1/projecttasks", JsonTool.ConvertFrom(projectTask));
        }
    }
}
