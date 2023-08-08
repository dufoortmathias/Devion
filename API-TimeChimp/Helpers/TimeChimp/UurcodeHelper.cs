namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpUurcodeHelper
{
    public static Boolean uurcodeExists(string code)
    {
        // connection with timechimp
        var client = new BearerTokenHttpClient();

        //get data from timechimp
        var response = client.GetAsync($"tasks");
        //convert data to timeTimeChimp object
        List<uurcodesTimeChimp> uurcodes = JsonTool.ConvertTo<List<uurcodesTimeChimp>>(response.Result);

        uurcodesTimeChimp uurcode = uurcodes.Find(uurcode => uurcode.code == code);

        if (uurcode != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static List<uurcodesTimeChimp> GetUurcodes()
    {
        // connection with timechimp
        var client = new BearerTokenHttpClient();

        //get data from timechimp
        var response = client.GetAsync($"tasks");
        //convert data to timeTimeChimp object
        List<uurcodesTimeChimp> uurcodes = JsonTool.ConvertTo<List<uurcodesTimeChimp>>(response.Result);

        return uurcodes;
    }

    public static uurcodesTimeChimp GetUurcode(string uurcodeId)
    {
        // connection with timechimp
        var client = new BearerTokenHttpClient();

        //get data frm timechimp
        var response = client.GetAsync($"tasks/{uurcodeId}");

        //convert data to timechimp object
        uurcodesTimeChimp uurcode = JsonTool.ConvertTo<uurcodesTimeChimp>(response.Result);

        return uurcode;
    }

    public static uurcodesTimeChimp CreateUurcode(uurcodesTimeChimp uurcode)
    {
        // connection with timechimp
        var client = new BearerTokenHttpClient();

        //get data from timechimp
        var response = client.PostAsync($"tasks", JsonTool.ConvertFrom(uurcode));
        //convert data to timeTimeChimp object
        uurcodesTimeChimp uurcodeResponse = JsonTool.ConvertTo<uurcodesTimeChimp>(response.Result);

        return uurcodeResponse;
    }

    public static uurcodesTimeChimp UpdateUurcode(uurcodesTimeChimp uurcode)
    {
        // connection with timechimp
        var client = new BearerTokenHttpClient();

        //get data from timechimp
        var response = client.PutAsync($"tasks", JsonTool.ConvertFrom(uurcode));
        //convert data to timeTimeChimp object
        uurcodesTimeChimp uurcodeResponse = JsonTool.ConvertTo<uurcodesTimeChimp>(response.Result);

        return uurcodeResponse;
    }
}