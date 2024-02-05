namespace Api.Devion.Helpers.ETS;

public class ETSCustomerHelper : ETSHelper
{
    public ETSCustomerHelper(FirebirdClientETS FBClient) : base(FBClient)
    {
    }

    //get customerids that are changed after the given date
    public string[] GetCustomerIdsChangedAfter(DateTime date)
    {
        //create query
        string query = $"SELECT KL_COD FROM KLPX WHERE DATE_CHANGED BETWEEN @date AND @dateNow";
        Dictionary<string, object> parameters = new()
        {
            {"@date",  date},
            {"@dateNow", DateTime.Now }
        };

        //get data from ETS
        string json = ETSClient.selectQuery(query, parameters) ?? throw new Exception("Error getting customerids from ETS with query: " + query);

        //get all ids from the json
        string[] ids = JsonTool.ConvertTo<CustomerETS[]>(json)
            .Select(customer => customer.KL_COD)
            .Where(x => x != null)
            .Cast<string>()
            .ToArray();
        return ids;
    }

    //get all customers
    public List<CustomerETS> GetCustomers()
    {
        //create query
        string query = "select K.*, BM.BM_OMS, BV.BVW_CODE, BC.BEL_CODE, R.REK_CODE from KLPX as K left join betaalmiddel as BM on K.KL_WIJZEBET = BM.BM_CODE left join tbl_betalingsvoorwaarde as BV on K.kl_betalingsvoorwaarde_id = BV.BVW_ID left join tbl_belasting_code as BC on K.kl_belasting_code_id = BC.BEL_ID left join tbl_rekening as R on K.kl_rekening_id = R.REK_ID; ";

        //get data from ETS
        string json = ETSClient.selectQuery(query) ?? throw new Exception("Error getting customers from ETS with query: " + query);


        //convert data to customerETS object
        List<CustomerETS> customers = JsonTool.ConvertTo<List<CustomerETS>>(json);
        return customers;
    }

    //get customer by customerId
    public CustomerETS? GetCustomer(string customerId)
    {
        //create query
        string query = $"select K.*, BM.BM_OMS, BV.BVW_CODE, BC.BEL_CODE, R.REK_CODE from KLPX as K left join betaalmiddel as BM on K.KL_WIJZEBET = BM.BM_CODE left join tbl_betalingsvoorwaarde as BV on K.kl_betalingsvoorwaarde_id = BV.BVW_ID left join tbl_belasting_code as BC on K.kl_belasting_code_id = BC.BEL_ID left join tbl_rekening as R on K.kl_rekening_id = R.REK_ID where K.KL_COD = @customer";
        Dictionary<string, object> parameters = new()
        {
            {"@customer",  customerId}
        };

        //get data form ETS
        string json = ETSClient.selectQuery(query, parameters) ?? throw new Exception("Error getting customer from ETS with query: " + query);

        //convert data to customerETS object
        CustomerETS? customer = JsonTool.ConvertTo<CustomerETS[]>(json).FirstOrDefault();
        return customer;
    }
}