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

        //get all ids from the json
        Int32[] ids = JsonTool.ConvertTo<contactsETS[]>(json)
            .Select(contact => contact.C_CODE)
            .Where(x => x != null)
            .Select(x => x.Value)
            .ToArray();
        return ids;
    }

    //get all contacts
    public List<contactsETS> GetContacts()
    {
        //get data form ETS
        var response = ETSClient.selectQuery("select C.CO_KLCOD, C.CO_TAV, C.CO_TAV2, C.CO_TEL, C.CO_FAX, C.CO_GSM, C.CO_EMAIL, C.CO_ACTIEF, F.FUT_OMSCHRIJVING from contact as C left join tbl_functie_taal as F on C.CO_FUNCTIE = F.FUT_ID");

        //convert data to contactETS object
        List<contactsETS> contacts = JsonTool.ConvertTo<List<contactsETS>>(response);
        return contacts;
    }

    //get contact by contactId
    public contactsETS GetContact(Int32 contactId)
    {
        //data from ETS
        var response = ETSClient.selectQuery($"select C.CO_KLCOD, C.CO_TAV, C.CO_TAV2, C.CO_TEL, C.CO_FAX, C.CO_GSM, C.CO_EMAIL, C.CO_ACTIEF, F.FUT_OMSCHRIJVING from contact as C left join tbl_functie_taal as F on C.CO_FUNCTIE = F.FUT_ID where c.C_CODE = {contactId}");

        //convert data to contactETS object
        contactsETS contact = JsonTool.ConvertTo<contactsETS[]>(response).FirstOrDefault();
        return contact;
    }
}