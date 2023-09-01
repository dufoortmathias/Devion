namespace Api.Devion.Helpers.ETS;

public class ETSMileageHelper : ETSHelper
{
    public ETSMileageHelper(FirebirdClientETS FBClient) : base(FBClient)
    {
    }

    //get all mileages
    public List<MileageETS> GetMileages()
    {
        //create query
        string query = "SELECT PLA_ID, PLA_KM, PLA_PROJECT, PLA_SUBPROJECT, PLA_PERSOON, PLA_KM_DERDEN, PLA_KM_VERGOEDING, PLA_START FROM tbl_planning";

        //get data form ETS
        string response = ETSClient.selectQuery(query) ?? throw new Exception("Error getting mileages from ETS with query: " + query);

        //convert data to mileageETS object
        List<MileageETS> mileages = JsonTool.ConvertTo<List<MileageETS>>(response);
        return mileages;
    }

    //add a mileage
    public MileageETS UpdateMileage(MileageETS mileage)
    {
        string queryGet;
        Dictionary<string, object> parametersGet;
        //create query to get mileages data from ETS that belong to current TimeChimp mileage
        if (mileage.PLA_SUBPROJECT != null && mileage.PLA_SUBPROJECT.Length == 0)
        {
            queryGet = $"SELECT * FROM tbl_planning WHERE PLA_PROJECT = @project AND PLA_SUBPROJECT IS NULL AND PLA_START LIKE @start AND PLA_PERSOON = @persoon;";
            parametersGet = new()
            {
                {"@project",  mileage.PLA_PROJECT ?? throw new Exception($"Mileage {mileage.PLA_ID} from ETS has no PLA_PROJECT")},
                {"@start", $"{mileage.PLA_START:yyyy-MM-dd}%" },
                {"@persoon", mileage.PLA_PERSOON ?? throw new Exception($"Mileage {mileage.PLA_ID} from ETS has no PLA_PERSOON")}
            };
        } else
        {

            queryGet = $"SELECT * FROM tbl_planning WHERE PLA_PROJECT = @project AND PLA_SUBPROJECT = @subproject AND PLA_START LIKE @start AND PLA_PERSOON = @persoon;";
            parametersGet = new()
            {
                {"@project",  mileage.PLA_PROJECT ?? throw new Exception($"Mileage {mileage.PLA_ID} from ETS has no PLA_PROJECT")},
                {"@subproject", mileage.PLA_SUBPROJECT ?? throw new Exception($"Mileage {mileage.PLA_ID} from ETS has no PLA_SUBPROJECT")},
                {"@start", $"{mileage.PLA_START:yyyy-MM-dd}%" },
                {"@persoon", mileage.PLA_PERSOON ?? throw new Exception($"Mileage {mileage.PLA_ID} from ETS has no PLA_PERSOON")}
            };
        }

        //get data from ETS
        string responseGet = ETSClient.selectQuery(queryGet, parametersGet);

        //check if response is succesfull
        if (responseGet == null)
        {
            throw new Exception("Error getting mileages from ETS with query: " + queryGet);
        }

        //convert first data  record to mileageETS object
        //the first mileage data object from ETS is always used to store new mileage registrations 
        MileageETS mileageETS = JsonTool.ConvertTo<List<MileageETS>>(responseGet).First() ?? throw new Exception("No time record found in ETS");

        mileage.PLA_KM += mileageETS.PLA_KM;
        mileage.PLA_ID = mileageETS.PLA_ID;


        //update query
        string query = $"UPDATE tbl_planning SET PLA_KM = @km, PLA_KM_DERDEN = @derden, PLA_KM_VERGOEDING = @vergoeding WHERE PLA_ID = @id;";
        Dictionary<string, object> parameters = new()
        {
            {"@km",  mileage.PLA_KM ?? throw new Exception($"Mileage {mileage.PLA_ID} from ETS has no PLA_KM")},
            {"@derden", mileage.PLA_KM_DERDEN ?? throw new Exception($"Mileage {mileage.PLA_ID} from ETS has no PLA_KM_DERDEN")},
            {"@vergoeding", mileage.PLA_KM_VERGOEDING ?? throw new Exception($"Mileage {mileage.PLA_ID} from ETS has no PLA_KM_VERGOEDING")},
            {"@id", mileage.PLA_ID ?? throw new Exception($"Mileage from ETS has no id")}
        };

        //send data to ETS
        ETSClient.ExecuteQuery(query, parameters);

        return mileage;
    }
}