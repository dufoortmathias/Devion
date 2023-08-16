namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpUurcodeHelper : TimeChimpHelper
{
    private FirebirdClientETS ETSClient;

    public TimeChimpUurcodeHelper(BearerTokenHttpClient clientTC, FirebirdClientETS clientETS) : base(clientTC)
    {
        ETSClient = clientETS;
    }

    //check if uurcode exists
    public Boolean uurcodeExists(string code)
    {
        //get data from timechimp
        String response = TCClient.GetAsync($"v1/tasks");

        //convert data to timeTimeChimp object
        List<uurcodesTimeChimp> uurcodes = JsonTool.ConvertTo<List<uurcodesTimeChimp>>(response);

        //search for uurcode
        uurcodesTimeChimp uurcode = uurcodes.Find(uurcode => uurcode.code == code);

        //check if uurcode exists
        if (uurcode != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //get all uurcodes
    public List<uurcodesTimeChimp> GetUurcodes()
    {
        //get data from timechimp
        String response = TCClient.GetAsync($"v1/tasks");

        //convert data to timeTimeChimp object
        List<uurcodesTimeChimp> uurcodes = JsonTool.ConvertTo<List<uurcodesTimeChimp>>(response);

        return uurcodes;
    }

    //get uurcode by id
    public uurcodesTimeChimp GetUurcode(Int32 uurcodeId)
    {
        //get data frm timechimp
        String response = TCClient.GetAsync($"v1/tasks/{uurcodeId}");

        //convert data to timechimp object
        uurcodesTimeChimp uurcode = JsonTool.ConvertTo<uurcodesTimeChimp>(response);

        return uurcode;
    }

    //create uurcode
    public uurcodesTimeChimp CreateUurcode(uurcodesTimeChimp uurcode)
    {
        //get data from timechimp
        String response = TCClient.PostAsync($"v1/tasks", JsonTool.ConvertFrom(uurcode));

        //convert data to timeTimeChimp object
        uurcodesTimeChimp uurcodeResponse = JsonTool.ConvertTo<uurcodesTimeChimp>(response);

        return uurcodeResponse;
    }


    public uurcodesTimeChimp UpdateUurcode(uurcodesTimeChimp uurcode)
    {
        var uurcode2 = GetUurcodes().Find(uur => uur.code == uurcode.code);

        //check if uurcode exists
        if (uurcode2 == null)
        {
            throw new Exception($"Error updating uurcode in timechimp with endpoint: v1/tasks/{uurcode.id}");
        }

        uurcode.id = uurcode2.id;
        //get data from timechimp
        String response = TCClient.PutAsync($"v1/tasks/", JsonTool.ConvertFrom(uurcode));

        //convert data to timeTimeChimp object
        uurcodesTimeChimp uurcodes = JsonTool.ConvertTo<uurcodesTimeChimp>(response);

        return uurcodes;
    }
}