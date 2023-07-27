namespace Api.Devion.Helpers.TimeChimp;

public static class TimeChimpCustomerHelper
{
    public static List<customerTimeChimp> GetCustomers()
    {
        // connection with timechimp
        var client = new BearerTokenHttpClient();
        var response = client.GetAsync("customers");
        List<customerTimeChimp> customers = JsonTool.ConvertTo<List<customerTimeChimp>>(response.Result);
        return customers;
    }

    public static customerTimeChimp CreateCustomer(customerTimeChimp customer)
    {
        //connection with timechimp
        var client = new BearerTokenHttpClient();
        var response = client.PostAsync("customers", JsonTool.ConvertFrom(customer));
        customerTimeChimp customerResponse = JsonTool.ConvertTo<customerTimeChimp>(response.Result);
        return customerResponse;
    }
}