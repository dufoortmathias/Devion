namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpContactHelper : TimeChimpHelper
{
    public TimeChimpContactHelper(BearerTokenHttpClient client) : base(client)
    {
    }

    //check if contact exists
    public bool ContactExists(ContactETS contactETS)
    {
        return GetContacts().Any(contact => contact.name.Equals(contactETS.CO_TAV));
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
        ContactTimeChimp originalContact;
        //check if contact has an id
        if (contact.id != null)
        {
            // get original contact
            string response = TCClient.GetAsync($"v1/contacts/{contact.id}");

            originalContact = JsonTool.ConvertTo<ContactTimeChimp>(response);
        }
        else
        {
            //search for original contact by name
            originalContact = GetContacts().Find(c => c.name.Equals(contact.name));

            //check if contact exists
            if (originalContact == null)
            {
                throw new Exception($"Error getting contact from TimeChimp with name: {contact.name}");
            }
        }

        // update original contact
        originalContact.name = contact.name;
        originalContact.jobTitle = contact.jobTitle;
        originalContact.email = contact.email;
        originalContact.phone = contact.phone;
        originalContact.useForInvoicing = contact.useForInvoicing;
        originalContact.active = contact.active;

        // get ids from customers
        List<int> customerIds = new();
        // get all customers
        List<CustomerTimeChimp> customers = new TimeChimpCustomerHelper(TCClient).GetCustomers();
        foreach (int customerId in contact.customerIds)
        {
            // get customer id
            string id = "00000" + customerId.ToString();
            //get the last 6 digits of the id
            id = id[^6..];
            foreach (CustomerTimeChimp customer in customers)
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
        string json = JsonTool.ConvertFrom(originalContact);

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