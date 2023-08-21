using Api.Devion.Models;

namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpContactHelper : TimeChimpHelper
{
    public TimeChimpContactHelper(BearerTokenHttpClient client) : base(client)
    {
    }

    //check if contact exists
    public bool ContactExists(ContactETS contactETS)
    {
        return GetContacts().Any(contact => contact.name.Equals(contactETS.CO_CONTACTPERSOON) && ((contact.email == null && contactETS.CO_EMAIL == null) || contact.email.Equals(contactETS.CO_EMAIL)));
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
            contact.id = GetContacts().Find(c => c.name.Equals(contact.name) && ((c.email != null && c.email.Equals(contact.email)) || c.email == null)).id;
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