namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpCustomerHelper : TimeChimpHelper
{
    public TimeChimpCustomerHelper(BearerTokenHttpClient client) : base(client)
    {
    }

    public Boolean CustomerExists(String customerId)
    {
        return GetCustomers().Any(customer => customer.relationId != null && customer.relationId.Equals(customerId));
    }

    public List<customerTimeChimp> GetCustomers()
    {
        // connection with timechimp
        var response = TCClient.GetAsync("v1/customers");
        List<customerTimeChimp> customers = JsonTool.ConvertTo<List<customerTimeChimp>>(response.Result);
        return customers;
    }

    public customerTimeChimp CreateCustomer(customerTimeChimp customer)
    {
        //connection with timechimp
        var response = TCClient.PostAsync("v1/customers", JsonTool.ConvertFrom(customer));
        customerTimeChimp customerResponse = JsonTool.ConvertTo<customerTimeChimp>(response.Result);
        return customerResponse;
    }

    public customerTimeChimp UpdateCustomer(customerTimeChimp customer)
    {
        //connection with timechimp
        customer.id = GetCustomers().Find(c => c.relationId.Equals(customer.relationId)).id;
        var response = TCClient.PutAsync("v1/customers", JsonTool.ConvertFrom(customer));
        customerTimeChimp customerResponse = JsonTool.ConvertTo<customerTimeChimp>(response.Result);
        return customerResponse;
    }

    public customerTimeChimp GetCustomer(String customerId)
    {
        //connection with timechimp
        var response = TCClient.GetAsync($"v1/customers/{customerId}");
        customerTimeChimp customerResponse = JsonTool.ConvertTo<customerTimeChimp>(response.Result);
        return customerResponse;
    }
}