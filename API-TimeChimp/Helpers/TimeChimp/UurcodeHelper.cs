namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpUurcodeHelper : TimeChimpHelper
{

    public TimeChimpUurcodeHelper(WebClient clientTC) : base(clientTC)
    {
    }

    //check if uurcode exists
    public bool uurcodeExists(string code)
    {
        //get data from timechimp
        string response = TCClient.GetAsync($"v1/tasks");

        //convert data to timeTimeChimp object
        List<UurcodeTimeChimp> uurcodes = JsonTool.ConvertTo<List<UurcodeTimeChimp>>(response);

        //search for uurcode
        UurcodeTimeChimp uurcode = uurcodes.Find(uurcode => uurcode.code == code);

        //check if uurcode exists
        return uurcode != null;
    }

    //get all uurcodes
    public List<UurcodeTimeChimp> GetUurcodes()
    {
        //get data from timechimp
        string response = TCClient.GetAsync($"v1/tasks");

        //convert data to timeTimeChimp object
        List<UurcodeTimeChimp> uurcodes = JsonTool.ConvertTo<List<UurcodeTimeChimp>>(response);

        return uurcodes;
    }

    //get uurcode by id
    public UurcodeTimeChimp GetUurcode(int uurcodeId)
    {
        //get data frm timechimp
        string response = TCClient.GetAsync($"v1/tasks/{uurcodeId}");

        //convert data to timechimp object
        UurcodeTimeChimp uurcode = JsonTool.ConvertTo<UurcodeTimeChimp>(response);

        return uurcode;
    }

    //create uurcode
    public UurcodeTimeChimp CreateUurcode(UurcodeTimeChimp uurcode)
    {
        //get data from timechimp
        string response = TCClient.PostAsync($"v1/tasks", JsonTool.ConvertFrom(uurcode));

        //convert data to timeTimeChimp object
        UurcodeTimeChimp uurcodeResponse = JsonTool.ConvertTo<UurcodeTimeChimp>(response);

        return uurcodeResponse;
    }


    public UurcodeTimeChimp UpdateUurcode(UurcodeTimeChimp uurcode)
    {
        UurcodeTimeChimp? uurcode2 = GetUurcodes().Find(uur => uur.code == uurcode.code);

        //check if uurcode exists
        if (uurcode2 == null)
        {
            throw new Exception($"Error updating uurcode in timechimp with endpoint: v1/tasks/{uurcode.id}");
        }

        uurcode.id = uurcode2.id;
        //get data from timechimp
        string response = TCClient.PutAsync($"v1/tasks/", JsonTool.ConvertFrom(uurcode));

        //convert data to timeTimeChimp object
        UurcodeTimeChimp uurcodes = JsonTool.ConvertTo<UurcodeTimeChimp>(response);

        return uurcodes;
    }
}