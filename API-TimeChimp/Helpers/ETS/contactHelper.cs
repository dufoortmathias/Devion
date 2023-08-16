namespace Api.Devion.Helpers.ETS;

public class ETSContactHelper : ETSHelper
{
    public ETSContactHelper(FirebirdClientETS FBClient) : base(FBClient)
    {
    }

    //get all contactids that are changed after the given date
    public Int32[] GetContactIdsChangedAfter(DateTime date)
    {
        //create query
        string query = $"SELECT C_CODE FROM contact WHERE DATE_CHANGED BETWEEN '{date.ToString("MM/dd/yyyy HH:mm")}' AND '{DateTime.Now.ToString("MM/dd/yyyy HH:mm")}'";

        //get data from ETS
        string json = ETSClient.selectQuery(query);

        //check if json is not empty
        if (json == null)
        {
            throw new Exception("Error getting contactids from ETS with query: " + query);
        }

        //get all ids from the json
        Int32[] ids = JsonTool.ConvertTo<ContactETS[]>(json)
            .Select(contact => contact.C_CODE)
            .Where(x => x != null)
            .Select(x => x.Value)
            .ToArray();
        return ids;
    }

    //get all contacts
    public List<ContactETS> GetContacts()
    {
        //create query
        string query = "select C.CO_KLCOD, C.CO_TAV, C.CO_TAV2, C.CO_TEL, C.CO_FAX, C.CO_GSM, C.CO_EMAIL, C.CO_ACTIEF, F.FUT_OMSCHRIJVING from contact as C left join tbl_functie_taal as F on C.CO_FUNCTIE = F.FUT_ID";

        //get data form ETS
        var response = ETSClient.selectQuery(query);

        //check if response is succesfull
        if (response == null)
        {
            throw new Exception("Error getting contacts from ETS with query: " + query);
        }

        //convert data to contactETS object
        List<ContactETS> contacts = JsonTool.ConvertTo<List<ContactETS>>(response);
        return contacts;
    }

    //get contact by contactId
    public ContactETS GetContact(Int32 contactId)
    {
        //create query
        var query = $"select C.CO_KLCOD, C.CO_TAV, C.CO_TAV2, C.CO_TEL, C.CO_FAX, C.CO_GSM, C.CO_EMAIL, C.CO_ACTIEF, F.FUT_OMSCHRIJVING from contact as C left join tbl_functie_taal as F on C.CO_FUNCTIE = F.FUT_ID where c.C_CODE = {contactId}";

        //data from ETS
        var response = ETSClient.selectQuery(query);

        //check if response is succesfull
        if (response == null)
        {
            throw new Exception("Error getting contact from ETS with query: " + query);
        }

        //convert data to contactETS object
        ContactETS contact = JsonTool.ConvertTo<ContactETS[]>(response).FirstOrDefault();
        return contact;
    }
}