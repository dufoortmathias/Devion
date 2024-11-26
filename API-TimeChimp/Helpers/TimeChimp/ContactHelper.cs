namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpContactHelper : TimeChimpHelper
{
    public TimeChimpContactHelper(WebClient client) : base(client)
    {
    }

    //check if contact exists
    public bool ContactExists(ContactETS contactETS)
    {
        return GetContacts().Any(contact => contact.Name != null && contact.Name.Equals(contactETS.CO_CONTACTPERSOON));
    }

    //get all contacts
    public List<ContactTimeChimp> GetContacts()
    {
        //get data from timechimp
        string response = TCClient.GetAsync("contacts?expand=customers");

        //convert data to contactTimeChimp object
        List<ContactTimeChimp> contacts = JsonTool.ConvertTo<ResponseTCContact>(response).Result.ToList();
        return contacts;
    }

    //create contact
    public ContactTimeChimp CreateContact(ContactTimeChimp contact)
    {
        //send data to timechimp
        string response = TCClient.PostAsync("contacts", JsonTool.ConvertFrom(contact));

        //convert response to contactTimeChimp object
        ContactTimeChimp contactResponse = JsonTool.ConvertTo<ResponseTCContact>(response).Result[0];
        return contactResponse;
    }

    public ContactTimeChimp UpdateContact(ContactTimeChimp contact)
    {
        if (contact.Id == 0)
        {
            // find TimeChimp id of contact
            ContactTimeChimp contactFound = GetContacts().Find(c => c.Customers.FirstOrDefault()!=null && (c.Customers.FirstOrDefault().Id == contact.Id||c.Customers.FirstOrDefault().Email == contact.Email)) ?? throw new Exception($"No contact found in TimeChimp with Id = {contact.Id} or Email = {contact.Email}");
            contact.Id = contactFound.Id;
        }

        // update contact TimeChimp
        string json = JsonTool.ConvertFrom(contact) ?? throw new Exception("Error converting contact to json");
        string response2 = TCClient.PutAsync($"contacts/{contact.Id}", json);

        ContactTimeChimp contactResponse = JsonTool.ConvertTo<ResponseTCContact>(response2).Result[0];
        return contactResponse;

    }
}