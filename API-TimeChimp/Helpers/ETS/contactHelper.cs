namespace Api.Devion.Helpers.ETS;

public class ETSContactHelper : ETSHelper
{
    public ETSContactHelper(FirebirdClientETS FBClient) : base(FBClient)
    {
    }

    //get all contactids that are changed after the given date
    public int[] GetContactIdsChangedAfter(DateTime date)
    {
        //create query
        string query = $"SELECT C_CODE FROM contact WHERE DATE_CHANGED BETWEEN @date AND @dateNow";
        Dictionary<string, object> parameters = new()
        {
            {"@date",  date},
            {"@dateNow", DateTime.Now }
        };

        //get data from ETS
        string json = ETSClient.selectQuery(query, parameters);

        //check if json is not empty
        if (json == null)
        {
            throw new Exception("Error getting contactids from ETS with query: " + query);
        }

        //get all ids from the json
        int[] ids = JsonTool.ConvertTo<ContactETS[]>(json)
            .Select(contact => contact.C_CODE)
            .Where(x => x != null)
            .Cast<int>()
            .ToArray();
        return ids;
    }

    //get all contacts
    public List<ContactETS> GetContacts()
    {
        //create query
        string query = "select C.*, F.FUT_OMSCHRIJVING from contact as C left join tbl_functie_taal as F on C.CO_FUNCTIE = F.FUT_ID";

        //get data form ETS
        string response = ETSClient.selectQuery(query);

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
    public ContactETS? GetContact(int contactId)
    {
        //create query
        string query = $"select C.*, F.FUT_OMSCHRIJVING from contact as C left join tbl_functie_taal as F on C.CO_FUNCTIE = F.FUT_ID where c.C_CODE = @contact";
        Dictionary<string, object> parameters = new()
        {
            {"@contact", contactId}
        };

        //data from ETS
        string response = ETSClient.selectQuery(query, parameters);

        //check if response is succesfull
        if (response == null)
        {
            throw new Exception("Error getting contact from ETS with query: " + query);
        }

        //convert data to contactETS object
        ContactETS? contact = JsonTool.ConvertTo<ContactETS[]>(response).FirstOrDefault();
        return contact;
    }
}