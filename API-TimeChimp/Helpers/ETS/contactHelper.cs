namespace Api.Devion.Helpers.ETS;

public static class ETSContactHelper
{
    public static Int32[] GetContactIds()
    {
        FirebirdClientETS client = new FirebirdClientETS();
        string query = "SELECT C_CODE FROM contact";
        string json = client.selectQuery(query);
        Int32[] ids = JsonTool.ConvertTo<contactsETS[]>(json)
            .Select(contact => contact.C_CODE)
            .Where(x => x != null)
            .Select(x => x.Value)
            .ToArray();
        return ids;
    }

    public static List<contactsETS> GetContacts()
    {
        // connection with ets
        var client = new FirebirdClientETS();
        var response = client.selectQuery("select C.CO_KLCOD, C.CO_TAV, C.CO_TAV2, C.CO_TEL, C.CO_FAX, C.CO_GSM, C.CO_EMAIL, C.CO_ACTIEF, F.FUT_OMSCHRIJVING from contact as C left join tbl_functie_taal as F on C.CO_FUNCTIE = F.FUT_ID");
        List<contactsETS> contacts = JsonTool.ConvertTo<List<contactsETS>>(response);
        return contacts;
    }

    public static contactsETS GetContact(Int32 contactId)
    {
        // connection with ets
        var client = new FirebirdClientETS();
        var response = client.selectQuery($"select C.CO_KLCOD, C.CO_TAV, C.CO_TAV2, C.CO_TEL, C.CO_FAX, C.CO_GSM, C.CO_EMAIL, C.CO_ACTIEF, F.FUT_OMSCHRIJVING from contact as C left join tbl_functie_taal as F on C.CO_FUNCTIE = F.FUT_ID where c.C_CODE = {contactId}");
        contactsETS contact = JsonTool.ConvertTo<List<contactsETS>>(response).First();
        return contact;
    }
}