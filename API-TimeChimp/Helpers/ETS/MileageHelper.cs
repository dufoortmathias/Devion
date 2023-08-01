namespace Api.Devion.Helpers.ETS;

public class ETSMileageHelper
{
    public static List<mileageETS> GetMileages()
    {
        var client = new FirebirdClientETS();

        var response = client.selectQuery("SELECT PLA_ID, PLA_KM, PLA_PROJECT, PLA_SUBPROJECT, PLA_PERSOON, PLA_KM_DERDEN, PLA_KM_VERGOEDING, PLA_START FROM tbl_planning");
        List<mileageETS> mileages = JsonTool.ConvertTo<List<mileageETS>>(response);
        return mileages;
    }

    public static string UpdateMileage(mileageETS mileage)
    {
        var client = new FirebirdClientETS();

        var queryGet = $"SELECT * FROM tbl_planning WHERE PLA_PROJECT = {mileage.PLA_PROJECT} AND PLA_SUBPROJECT = {mileage.PLA_SUBPROJECT};";
        var responseGet = client.selectQuery(queryGet);
        List<mileageETS> mileages = JsonTool.ConvertTo<List<mileageETS>>(responseGet);
        if (mileages.Count == 0)
        {
            return "No mileage found";
        }
        foreach (mileageETS mileageETS in mileages)
        {
            if (mileageETS.PLA_START.Date == mileage.PLA_START.Date && mileageETS.PLA_PERSOON == mileage.PLA_PERSOON)
            {
                mileage.PLA_ID = mileageETS.PLA_ID;
            }
        }

        //update query
        var query = $"UPDATE tbl_planning SET PLA_KM_HEEN_TERUG = 0, PLA_KM = {mileage.PLA_KM}, PLA_KM_DERDEN = '{mileage.PLA_KM_DERDEN}', PLA_KM_VERGOEDING = '{mileage.PLA_KM_VERGOEDING}' WHERE PLA_ID = {mileage.PLA_ID};";
        var response = client.updateQuery(query);
        return response;
    }
}