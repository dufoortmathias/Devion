namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpUurcodeHelper : TimeChimpHelper
{
    private FirebirdClientETS ETSClient;

    public TimeChimpUurcodeHelper(BearerTokenHttpClient clientTC, FirebirdClientETS clientETS) : base(clientTC)
    {
        ETSClient = clientETS;
    }

    public Boolean uurcodeExists(string code)
    {
        //get data from timechimp
        var response = TCClient.GetAsync($"v1/tasks");
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
    public List<uurcodesTimeChimp> GetUurcodes()
    {
        //get data from timechimp
        var response = TCClient.GetAsync($"v1/tasks");
        //convert data to timeTimeChimp object
        List<uurcodesTimeChimp> uurcodes = JsonTool.ConvertTo<List<uurcodesTimeChimp>>(response.Result);

        return uurcodes;
    }

    public uurcodesTimeChimp GetUurcode(string uurcodeId)
    {
        //get data frm timechimp
        var response = TCClient.GetAsync($"v1/tasks/{uurcodeId}");

        //convert data to timechimp object
        uurcodesTimeChimp uurcode = JsonTool.ConvertTo<uurcodesTimeChimp>(response.Result);

        return uurcode;
    }

    public uurcodesTimeChimp CreateUurcode(uurcodesTimeChimp uurcode)
    {
        //get data from timechimp
        var response = TCClient.PostAsync($"v1/tasks", JsonTool.ConvertFrom(uurcode));
        //convert data to timeTimeChimp object
        uurcodesTimeChimp uurcodeResponse = JsonTool.ConvertTo<uurcodesTimeChimp>(response.Result);

        return uurcodeResponse;
    }

    public List<uurcodesTimeChimp> UpdateUurcodes()
    {
        List<uurcodesETS> uurcodes = new ETSUurcodeHelper(ETSClient).GetUurcodes();
        List<uurcodesTimeChimp> uurcodesUpdated = new List<uurcodesTimeChimp>();

        foreach (uurcodesETS uurcode in uurcodes)
        {
            if (!uurcodeExists(uurcode.UR_COD))
            {
                uurcodesTimeChimp uurcodeTimeChimp = new uurcodesTimeChimp(uurcode);
                var response = new TimeChimpUurcodeHelper(TCClient, ETSClient).CreateUurcode(uurcodeTimeChimp);
                uurcodesUpdated.Add(response);
            }
        }

        return uurcodesUpdated;
    }
}