namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpContactHelper
{
    public static Boolean ContactExists(contactsETS contactETS)
    {
        return GetContacts().Any(contact => contact.email.Equals(contactETS.CO_EMAIL));
    }

    public static List<contactsTimeChimp> GetContacts()
    {
        // connection with timechimp
        var client = new BearerTokenHttpClient();
        var response = client.GetAsync("contacts");
        List<contactsTimeChimp> contacts = JsonTool.ConvertTo<List<contactsTimeChimp>>(response.Result);
        return contacts;
    }

    public static contactsTimeChimp CreateContact(contactsTimeChimp contact)
    {
        //connection with timechimp
        var client = new BearerTokenHttpClient();
        var response = client.PostAsync("contacts", JsonTool.ConvertFrom(contact));
        contactsTimeChimp contactResponse = JsonTool.ConvertTo<contactsTimeChimp>(response.Result);
        return contactResponse;
    }

    public static contactsTimeChimp UpdateContact(contactsTimeChimp contact)
    {
        //connection with timechimp
        var client = new BearerTokenHttpClient();
        var response = client.GetAsync($"contacts/{contact.id}");
        contactsTimeChimp originalContact = JsonTool.ConvertTo<contactsTimeChimp>(response.Result);
        //checking with original contact
        if (contact.name != originalContact.name)
        {
            originalContact.name = contact.name;
        }
        else if (contact.jobTitle != originalContact.jobTitle)
        {
            originalContact.jobTitle = contact.jobTitle;
        }
        else if (contact.email != originalContact.email)
        {
            originalContact.email = contact.email;
        }
        else if (contact.phone != originalContact.phone)
        {
            originalContact.phone = contact.phone;
        }
        else if (contact.useForInvoicing != originalContact.useForInvoicing)
        {
            originalContact.useForInvoicing = contact.useForInvoicing;
        }
        else if (contact.active != originalContact.active)
        {
            originalContact.active = contact.active;
        }

        //checking if customerIds are equal
        bool areEqual = originalContact.customerIds.SequenceEqual(contact.customerIds);
        if (!areEqual)
        {
            originalContact.customerIds = contact.customerIds;
        }

        var json = JsonTool.ConvertFrom(originalContact);
        var response2 = client.PutAsync($"contacts", json);
        contactsTimeChimp contactResponse = JsonTool.ConvertTo<contactsTimeChimp>(response2.Result);
        return contactResponse;

    }
}