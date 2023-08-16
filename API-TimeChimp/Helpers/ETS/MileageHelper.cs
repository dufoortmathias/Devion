using Api.Devion.Models;

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
        var response = ETSClient.selectQuery(query);

        //check if response is succesfull
        if (response == null)
        {
            throw new Exception("Error getting mileages from ETS with query: " + query);
        }

        //convert data to mileageETS object
        List<MileageETS> mileages = JsonTool.ConvertTo<List<MileageETS>>(response);
        return mileages;
    }

    //add a mileage
    public MileageETS UpdateMileage(MileageETS mileage)
    {
        //create query to get projectid and subprojectid for the mileage
        var queryGet = $"SELECT * FROM tbl_planning WHERE PLA_PROJECT = '{mileage.PLA_PROJECT}' AND PLA_SUBPROJECT = '{mileage.PLA_SUBPROJECT}' AND PLA_START LIKE '{mileage.PLA_START.ToString("yyyy-MM-dd")}%' AND PLA_PERSOON = '{mileage.PLA_PERSOON}';";

        //get data from ETS
        var responseGet = ETSClient.selectQuery(queryGet);

        //check if response is succesfull
        if (responseGet == null)
        {
            throw new Exception("Error getting mileages from ETS with query: " + queryGet);
        }

        //convert data to mileageETS object
        MileageETS mileageETS = JsonTool.ConvertTo<List<MileageETS>>(responseGet).First();

        mileage.PLA_KM += mileageETS.PLA_KM;
        mileage.PLA_ID = mileageETS.PLA_ID;


        //update query
        var query = $"UPDATE tbl_planning SET PLA_KM = {mileage.PLA_KM}, PLA_KM_DERDEN = '{mileage.PLA_KM_DERDEN}', PLA_KM_VERGOEDING = '{mileage.PLA_KM_VERGOEDING}' WHERE PLA_ID = {mileage.PLA_ID};";

        //send data to ETS
        var response = ETSClient.updateQuery(query);

        //check if response is succesfull
        if (response == null)
        {
            throw new Exception("Error updating mileage in ETS with query: " + query);
        }

        return mileage;
    }
}