namespace Api.Devion.Helpers.TimeChimp;

public class TimeChimpCustomerHelper : TimeChimpHelper
{
    public TimeChimpCustomerHelper(WebClient client) : base(client)
    {
    }

    //check if customer exists
    public bool CustomerExists(string customerId)
    {
        return GetCustomers().Any(customer => customer.RelationId != null && customer.RelationId.Equals(customerId));
    }

    //get all customers
    public List<CustomerTimeChimp> GetCustomers()
    {
        //get data from timechimp
        string response = TCClient.GetAsync("customers?$top=10000");

        //convert data to customerTimeChimp object
        List<CustomerTimeChimp> customers = JsonTool.ConvertTo<ResponseTCCustomer>(response).Result.ToList();
        return customers;
    }

    //create customer
    public CustomerTimeChimp CreateCustomer(CustomerTimeChimp customer)
    {
        //send data to timechimp
        string response = TCClient.PostAsync("customers", JsonTool.ConvertFrom(customer));

        //convert response to customerTimeChimp object
        CustomerTimeChimp customerResponse = JsonTool.ConvertTo<ResponseTCCustomer>(response).Result[0];
        return customerResponse;
    }

    //update customer
    public CustomerTimeChimp UpdateCustomer(CustomerTimeChimp customer)
    {
        //get id from customer
        CustomerTimeChimp customerFound = GetCustomers().Find(c => c.RelationId != null && c.RelationId.Equals(customer.RelationId)) ?? throw new Exception($"No customer found in timechimp with id = {customer.RelationId}");
        customer.Id = customerFound.Id;

        //send data to timechimp
        string response = TCClient.PutAsync($"customers/{customer.Id}", JsonTool.ConvertFrom(customer));

        //convert response to customerTimeChimp object
        CustomerTimeChimp customerResponse = JsonTool.ConvertTo<ResponseTCCustomer>(response).Result[0];
        return customerResponse;
    }

    //get customer by id
    public CustomerTimeChimp GetCustomer(int customerId)
    {
        //get data form timechimp
        string response = TCClient.GetAsync($"customers/{customerId}");

        //convert data to customerTimeChimp object
        CustomerTimeChimp customerResponse = JsonTool.ConvertTo<ResponseTCCustomer>(response).Result[0];
        return customerResponse;
    }

    //get customer by klantnummer
    public CustomerTimeChimp GetContactByKlantnr(int klantnr)
    {
        //get data form timechimp
        string response = TCClient.GetAsync($"customers?$filter=relationId eq {klantnr}");

        //convert data to customerTimeChimp object
        CustomerTimeChimp customerResponse = JsonTool.ConvertTo<ResponseTCCustomer>(response).Result[0];
        return customerResponse;
    }
}