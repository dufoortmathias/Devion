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
        var response = TCClient.GetAsync("v1/customers");

        //check if response is succesfull
        if (!response.IsCompletedSuccessfully)
        {
            throw new Exception("Error getting all customers from timechimp with endpoint: v1/customers");
        }
        //convert data to customerTimeChimp object
        List<customerTimeChimp> customers = JsonTool.ConvertTo<List<customerTimeChimp>>(response.Result);
        return customers;
    }

    //create customer
    public customerTimeChimp CreateCustomer(customerTimeChimp customer)
    {
        //send data to timechimp
        var response = TCClient.PostAsync("v1/customers", JsonTool.ConvertFrom(customer));

        //check if response is succesfull
        if (!response.IsCompletedSuccessfully)
        {
            throw new Exception("Error creating customer in timechimp with endpoint: v1/customers");
        }
        //convert response to customerTimeChimp object
        customerTimeChimp customerResponse = JsonTool.ConvertTo<customerTimeChimp>(response.Result);
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
        var response = TCClient.PutAsync("v1/customers", JsonTool.ConvertFrom(customer));

        //check if response is succesfull
        if (!response.IsCompletedSuccessfully)
        {
            throw new Exception("Error updating customer in timechimp with endpoint: v1/customers");
        }

        //convert response to customerTimeChimp object
        customerTimeChimp customerResponse = JsonTool.ConvertTo<customerTimeChimp>(response.Result);
        return customerResponse;
    }

    //get customer by id
    public customerTimeChimp GetCustomer(String customerId)
    {
        //get data form timechimp
        var response = TCClient.GetAsync($"v1/customers/{customerId}");

        //check if response is succesfull
        if (!response.IsCompletedSuccessfully)
        {
            throw new Exception($"Error getting customer from timechimp with endpoint: v1/customers/{customerId}");
        }

        //convert data to customerTimeChimp object
        customerTimeChimp customerResponse = JsonTool.ConvertTo<customerTimeChimp>(response.Result);
        return customerResponse;
    }
}