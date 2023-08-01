namespace Api.Devion.Helpers.ETS;

public class ETSMileageHelper
{
    public static List<mileageETS> GetMileages()
    {
        var client = new FirebirdClientETS();

        var response = client.selectQuery("SELECT PLA_KM, PLA_PROJECT, PLA_SUBPROJECT, PLA_PERSOON, PLA_KM_DERDEN, PLA_KM_VERGOEDING, PLA_START FROM tbl_planning");
        Console.WriteLine(response);
        List<mileageETS> mileages = JsonTool.ConvertTo<List<mileageETS>>(response);
        return mileages;
    }
}