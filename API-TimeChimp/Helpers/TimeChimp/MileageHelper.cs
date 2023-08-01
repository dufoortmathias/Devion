namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpMileageHelper
{
    public static List<mileageTimeChimp> GetMileages()
    {
        var client = new BearerTokenHttpClient();

        var response = client.GetAsync("mileage").Result;
        Console.WriteLine(response);
        List<mileageTimeChimp> mileages = JsonTool.ConvertTo<List<mileageTimeChimp>>(response);
        return mileages;
    }
}