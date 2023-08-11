namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpCustomerHelper : TimeChimpHelper
{
    public TimeChimpCustomerHelper(BearerTokenHttpClient client) : base(client)
    {
    }

    //check if customer exists
    public Boolean CustomerExists(String customerId)
    {
        return GetCustomers().Any(customer => customer.relationId != null && customer.relationId.Equals(customerId));
    }

    //get all customers
    public List<customerTimeChimp> GetCustomers()
    {
        //get data from timechimp
        String response = TCClient.GetAsync("v1/customers");

        //convert data to customerTimeChimp object
        List<customerTimeChimp> customers = JsonTool.ConvertTo<List<customerTimeChimp>>(response);
        return customers;
    }

    //create customer
    public customerTimeChimp CreateCustomer(customerTimeChimp customer)
    {
        //send data to timechimp
        String response = TCClient.PostAsync("v1/customers", JsonTool.ConvertFrom(customer));

        //convert response to customerTimeChimp object
        customerTimeChimp customerResponse = JsonTool.ConvertTo<customerTimeChimp>(response);
        return customerResponse;
    }

    //update customer
    public customerTimeChimp UpdateCustomer(customerTimeChimp customer)
    {
        //get id from customer
        customer.id = GetCustomers().Find(c => c.relationId.Equals(customer.relationId)).id;

        //check if customer exists
        if (customer.id == null)
        {
            throw new Exception("Customer does not exist in timechimp");
        }

        //send data to timechimp
        String response = TCClient.PutAsync("v1/customers", JsonTool.ConvertFrom(customer));

        //convert response to customerTimeChimp object
        customerTimeChimp customerResponse = JsonTool.ConvertTo<customerTimeChimp>(response);
        return customerResponse;
    }

    //get customer by id
    public customerTimeChimp GetCustomer(String customerId)
    {
        //get data form timechimp
        String response = TCClient.GetAsync($"v1/customers/{customerId}");

        //convert data to customerTimeChimp object
        customerTimeChimp customerResponse = JsonTool.ConvertTo<customerTimeChimp>(response);
        return customerResponse;
    }
}