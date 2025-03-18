namespace Api.Devion.Helpers.ETS
{
    public class ETSProjectHelper : ETSHelper
    {
        public ETSProjectHelper(FirebirdClientETS FBClient) : base(FBClient)
        {
        }

        // get project by projectId
        public ProjectETS GetProject(string projectId)
        {
            //create query
            string query = $"SELECT * FROM PROJPX WHERE PR_NR = @id";
            Dictionary<string, object> parameters = new()
        {
            {"@id",  projectId}
        };

            //get data from ETS
            string json = ETSClient.selectQuery(query, parameters);

            //check if json is not empty
            if (json == null)
            {
                throw new Exception("Error getting project from ETS with query: " + query);
            }

            //convert data to projectETS object
            //Console.WriteLine(json);
            ProjectETS project = JsonTool.ConvertTo<ProjectETS[]>(json).First();
            return project;
        }

        //get all subprojects from a project
        public List<SubprojectETS> GetSubprojects(string projectId)
        {
            //create query
            string query = $"SELECT * FROM SUBPROJ WHERE VOLNR like @id";
            Dictionary<string, object> parameters = new()
        {
            {"@id",  $"{projectId}%"}
        };

            //get data from ETS
            string json = ETSClient.selectQuery(query, parameters);

            //check if json is not empty

            //convert data to subprojectETS object
            List<SubprojectETS> subprojects = JsonTool.ConvertTo<List<SubprojectETS>>(json);
            return subprojects;
        }

        //get all projectids that are changed after the given date
        public string[] GetProjectIdsChangedAfter(DateTime date)
        {
            //create query
            string query = $"SELECT PROJPX.PR_NR FROM PROJPX WHERE DATE_CHANGED BETWEEN @date AND @dateNow";
            Dictionary<string, object> parameters = new()
            {
                {"@date",  date },
                {"@dateNow", DateTime.Now }
            };

            //get data from ETS
            string json = ETSClient.selectQuery(query, parameters);

            //check if json is not empty
            if (json == null)
            {
                throw new Exception("Error getting projectids from ETS with query: " + query);
            }

            //get all ids from the json
            string[] ids = JsonTool.ConvertTo<ProjectETS[]>(json)
                .Select(project => project.PR_NR)
                .Where(x => x != null)
                .Cast<string>()
                .ToArray();

            return ids;
        }
    }
}
