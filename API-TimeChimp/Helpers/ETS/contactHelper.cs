namespace Api.Devion.Helpers.ETS;

public static class ETSContactHelper
{
    public static List<contactsETS> GetContacts()
    {
        // connection with ets
        var client = new FirebirdClientETS();
        var response = client.selectQuery("select C.CO_KLCOD, C.CO_TAV, C.CO_TAV2, C.CO_TEL, C.CO_FAX, C.CO_GSM, C.CO_EMAIL, C.CO_ACTIEF, F.FUT_OMSCHRIJVING from contact as C left join tbl_functie_taal as F on C.CO_FUNCTIE = F.FUT_ID");
        List<contactsETS> contacts = JsonConvert.DeserializeObject<List<contactsETS>>(response);
        return contacts;
    }
}