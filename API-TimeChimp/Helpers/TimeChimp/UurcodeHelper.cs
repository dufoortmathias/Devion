namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpUurcodeHelper : TimeChimpHelper
{

    public TimeChimpUurcodeHelper(WebClient clientTC) : base(clientTC)
    {
    }

    //check if uurcode exists
    public bool UurcodeExists(string code)
    {
        //get data from timechimp
        string response = TCClient.GetAsync($"tasks");

        //convert data to timeTimeChimp object
        List<UurcodeTimeChimp> uurcodes = JsonTool.ConvertTo<ResponseTCUurcode>(response).Result.ToList();

        //search for uurcode
        UurcodeTimeChimp? uurcode = uurcodes.Find(uurcode => uurcode.Code == code);

        //check if uurcode exists
        return uurcode != null;
    }

    //get all uurcodes
    public List<UurcodeTimeChimp> GetUurcodes()
    {
        //get data from timechimp
        string response = TCClient.GetAsync($"tasks");

        //convert data to timeTimeChimp object
        ResponseTCUurcode uurcodes = JsonTool.ConvertTo<ResponseTCUurcode>(response);

        return uurcodes.Result.ToList();
    }

    //get uurcode by id
    public UurcodeTimeChimp GetUurcode(int? uurcodeId)
    {
        if (uurcodeId == null)
        {
            throw new Exception("Error getting uurcode from timechimp with endpoint: v1/tasks/{uurcodeId}");
        }
        //get data frm timechimp
        string response = TCClient.GetAsync($"tasks/{uurcodeId}");

        //convert data to timechimp object
        ResponseTCUurcode uurcode = JsonTool.ConvertTo<ResponseTCUurcode>(response);

        return uurcode.Result[0];
    }

    //create uurcode
    public UurcodeTimeChimp CreateUurcode(UurcodeTimeChimp uurcode)
    {
        //get data from timechimp
        string response = TCClient.PostAsync($"tasks", JsonTool.ConvertFrom(uurcode));

        //convert data to timeTimeChimp object
        UurcodeTimeChimp uurcodeResponse = JsonTool.ConvertTo<ResponseTCUurcode>(response).Result[0];

        return uurcodeResponse;
    }


    public UurcodeTimeChimp UpdateUurcode(UurcodeTimeChimp uurcode)
    {
        UurcodeTimeChimp? uurcode2 = GetUurcodes().Find(uur => uur.Code == uurcode.Code) ?? throw new Exception($"Error updating uurcode in timechimp with endpoint: v1/tasks/{uurcode.Id}");
        uurcode.Id = uurcode2.Id;

        //check each value if equal
        List<Patch> patches = new List<Patch>();
        if (uurcode.Code != uurcode2.Code)
        {
            patches.Add(new Patch("replace", "/code", uurcode.Code));
        }

        if (uurcode.Active != uurcode2.Active)
        {
            patches.Add(new Patch("replace", "/active", uurcode.Active.ToString()));
        }

        if (uurcode.Billable != uurcode2.Billable)
        {
            patches.Add(new Patch("replace", "/billable", uurcode.Billable.ToString()));
        }

        if (uurcode.Name != uurcode2.Name)
        {
            patches.Add(new Patch("replace", "/name", uurcode.Name));
        }
        //get data from timechimp
        string response = TCClient.PatchAsync($"tasks/{uurcode2.Id}", JsonTool.ConvertFrom(patches));

        //convert data to timeTimeChimp object
        UurcodeTimeChimp uurcodes = JsonTool.ConvertTo<ResponseTCUurcode>(response).Result[0];

        return uurcodes;
    }
}