namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpCustomerHelper : TimeChimpHelper
{
    public TimeChimpCustomerHelper(WebClient client) : base(client)
    {
    }

    //check if customer exists
    public bool CustomerExists(string customerId)
    {
        return GetCustomers().Any(customer => customer.relationId != null && customer.relationId.Equals(customerId));
    }

    //get all customers
    public List<CustomerTimeChimp> GetCustomers()
    {
        //get data from timechimp
        string response = TCClient.GetAsync("v1/customers");

        //convert data to customerTimeChimp object
        List<CustomerTimeChimp> customers = JsonTool.ConvertTo<List<CustomerTimeChimp>>(response);
        return customers;
    }

    //create customer
    public CustomerTimeChimp CreateCustomer(CustomerTimeChimp customer)
    {
        //send data to timechimp
        string response = TCClient.PostAsync("v1/customers", JsonTool.ConvertFrom(customer));

        //convert response to customerTimeChimp object
        CustomerTimeChimp customerResponse = JsonTool.ConvertTo<CustomerTimeChimp>(response);
        return customerResponse;
    }

    //update customer
    public CustomerTimeChimp UpdateCustomer(CustomerTimeChimp customer)
    {
        //get id from customer
        CustomerTimeChimp customerFound = GetCustomers().Find(c => c.relationId != null && c.relationId.Equals(customer.relationId)) ?? throw new Exception($"No customer found in timechimp with id = {customer.relationId}");
        customer.id = customerFound.id;

        //check if customer exists
        if (customer.id == null)
        {
            throw new Exception("Customer does not exist in timechimp");
        }

        //send data to timechimp
        string response = TCClient.PutAsync("v1/customers", JsonTool.ConvertFrom(customer));

        //convert response to customerTimeChimp object
        CustomerTimeChimp customerResponse = JsonTool.ConvertTo<CustomerTimeChimp>(response);
        return customerResponse;
    }

    //get customer by id
    public CustomerTimeChimp GetCustomer(int customerId)
    {
        //get data form timechimp
        string response = TCClient.GetAsync($"v1/customers/{customerId}");

        //convert data to customerTimeChimp object
        CustomerTimeChimp customerResponse = JsonTool.ConvertTo<CustomerTimeChimp>(response);
        return customerResponse;
    }
}