namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpContactHelper : TimeChimpHelper
{
    public TimeChimpContactHelper(BearerTokenHttpClient client) : base(client)
    {
    }

    public Boolean ContactExists(contactsETS contactETS)
    {
        return GetContacts().Any(contact => contact.name.Equals(contactETS.CO_TAV));
    }

    public List<contactsTimeChimp> GetContacts()
    {
        // connection with timechimp
        var response = TCClient.GetAsync("v1/contacts");
        List<contactsTimeChimp> contacts = JsonTool.ConvertTo<List<contactsTimeChimp>>(response.Result);
        return contacts;
    }

    public contactsTimeChimp CreateContact(contactsTimeChimp contact)
    {
        //connection with timechimp
        var response = TCClient.PostAsync("v1/contacts", JsonTool.ConvertFrom(contact));
        contactsTimeChimp contactResponse = JsonTool.ConvertTo<contactsTimeChimp>(response.Result);
        return contactResponse;
    }

    public contactsTimeChimp UpdateContact(contactsTimeChimp contact)
    {
        // receive original contact from TimeChimp
        contactsTimeChimp originalContact;
        if (contact.id != null)
        {
            var response = TCClient.GetAsync($"v1/contacts/{contact.id}").Result;
            originalContact = JsonTool.ConvertTo<contactsTimeChimp>(response);
        }
        else
        {
            originalContact = GetContacts().Find(c => c.name.Equals(contact.name));
        }

        // update original contact
        originalContact.name = contact.name;
        originalContact.jobTitle = contact.jobTitle;
        originalContact.email = contact.email;
        originalContact.phone = contact.phone;
        originalContact.useForInvoicing = contact.useForInvoicing;
        originalContact.active = contact.active;

        // get ids from customers
        var customerIds = new List<int>();
        List<customerTimeChimp> customers = new TimeChimpCustomerHelper(TCClient).GetCustomers();
        foreach (var customerId in contact.customerIds)
        {
            var id = "00000" + customerId.ToString();
            id = id.Substring(id.Length - 6);
            foreach (customerTimeChimp customer in customers)
            {
                if (customer.relationId.Equals(id))
                {
                    customerIds.Add(customer.id.Value);
                    break;
                }
            }
        }
        originalContact.customerIds = customerIds.ToArray();

        // update contact TimeChimp
        var json = JsonTool.ConvertFrom(originalContact);
        var response2 = TCClient.PutAsync("v1/contacts", json).Result;
        contactsTimeChimp contactResponse = JsonTool.ConvertTo<contactsTimeChimp>(response2);
        return contactResponse;

    }
}