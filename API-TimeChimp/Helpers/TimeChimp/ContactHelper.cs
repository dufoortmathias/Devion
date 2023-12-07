namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpContactHelper : TimeChimpHelper
{
    public TimeChimpContactHelper(WebClient client) : base(client)
    {
    }

    //check if contact exists
    public bool ContactExists(ContactETS contactETS)
    {
        return GetContacts().Any(contact => contact.name != null && contact.name.Equals(contactETS.CO_CONTACTPERSOON) && ((contact.email == null && contactETS.CO_EMAIL == null) || (contact.email != null && contact.email.Equals(contactETS.CO_EMAIL))));
    }

    //get all contacts
    public List<ContactTimeChimp> GetContacts()
    {
        //get data from timechimp
        string response = TCClient.GetAsync("v1/contacts");

        //convert data to contactTimeChimp object
        List<ContactTimeChimp> contacts = JsonTool.ConvertTo<List<ContactTimeChimp>>(response);
        return contacts;
    }

    //create contact
    public ContactTimeChimp CreateContact(ContactTimeChimp contact)
    {
        //send data to timechimp
        string response = TCClient.PostAsync("v1/contacts", JsonTool.ConvertFrom(contact));

        //convert response to contactTimeChimp object
        ContactTimeChimp contactResponse = JsonTool.ConvertTo<ContactTimeChimp>(response);
        return contactResponse;
    }

    public ContactTimeChimp UpdateContact(ContactTimeChimp contact)
    {
        if (contact.id == null)
        {
            // find TimeChimp id of contact
            ContactTimeChimp contactFound = GetContacts().Find(c => c.name != null && c.name.Equals(contact.name) && ((c.email != null && c.email.Equals(contact.email)) || c.email == null)) ?? throw new Exception($"No contact found in TimeChimp with name = {contact.name}" + (contact.email == null ? "" : $" and email = {contact.email}"));
            contact.id = contactFound.id;
        }

        // update contact TimeChimp
        string json = JsonTool.ConvertFrom(contact);

        //check if json is not empty
        if (json == null)
        {
            throw new Exception("Error converting contact to json");
        }

        string response2 = TCClient.PutAsync("v1/contacts", json);

        ContactTimeChimp contactResponse = JsonTool.ConvertTo<ContactTimeChimp>(response2);
        return contactResponse;

    }
}