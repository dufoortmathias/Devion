namespace Api.Devion.Helpers.ETS;

public class ETSCustomerHelper : ETSHelper
{
    public ETSCustomerHelper(FirebirdClientETS FBClient) : base(FBClient)
    {
    }

    //get customerids that are changed after the given date
    public String[] GetCustomerIdsChangedAfter(DateTime date)
    {
        //create query
        string query = $"SELECT KL_COD FROM KLPX WHERE DATE_CHANGED BETWEEN '{date.ToString("MM/dd/yyyy HH:mm")}' AND '{DateTime.Now.ToString("MM/dd/yyyy HH:mm")}'";

        //get data from ETS
        string json = ETSClient.selectQuery(query);

        //check if json is not empty
        if (json == null)
        {
            throw new Exception("Error getting customerids from ETS with query: " + query);
        }

        //get all ids from the json
        String[] ids = JsonTool.ConvertTo<CustomersETS[]>(json)
            .Select(customer => customer.KL_COD)
            .ToArray();
        return ids;
    }

    //get all customers
    public List<CustomersETS> GetCustomers()
    {
        //create query
        string query = "select K.KL_COD, K.KL_NAM, K.KL_OPV, K.KL_STR, K.KL_PNR, K.KL_WPL, K.KL_LND, K.KL_TEL, K.KL_FAX, K.KL_TEX, K.KL_EMAIL, K.KL_WEBPAGE, K.KL_T, K.KL_BTW, K.KL_TYP, K.KL_BOE, K.KL_VRIJ1, K.KL_MNT, BM.BM_OMS, BV.BVW_CODE, BC.BEL_CODE, R.REK_CODE, KL_SLECHTBET from KLPX as K left join betaalmiddel as BM on K.KL_WIJZEBET = BM.BM_CODE left join tbl_betalingsvoorwaarde as BV on K.kl_betalingsvoorwaarde_id = BV.BVW_ID left join tbl_belasting_code as BC on K.kl_belasting_code_id = BC.BEL_ID left join tbl_rekening as R on K.kl_rekening_id = R.REK_ID; ";

        //get data from ETS
        string json = ETSClient.selectQuery(query);

        //check if json is not empty
        if (json == null)
        {
            throw new Exception("Error getting customers from ETS with query: " + query);
        }


        //convert data to customerETS object
        List<CustomersETS> customers = JsonTool.ConvertTo<List<CustomersETS>>(json);
        return customers;
    }

    //get customer by customerId
    public CustomersETS GetCustomer(String customerId)
    {
        //create query
        string query = $"select K.KL_COD, K.KL_NAM, K.KL_OPV, K.KL_STR, K.KL_PNR, K.KL_WPL, K.KL_LND, K.KL_TEL, K.KL_FAX, K.KL_TEX, K.KL_EMAIL, K.KL_WEBPAGE, K.KL_T, K.KL_BTW, K.KL_TYP, K.KL_BOE, K.KL_VRIJ1, K.KL_MNT, BM.BM_OMS, BV.BVW_CODE, BC.BEL_CODE, R.REK_CODE, KL_SLECHTBET from KLPX as K left join betaalmiddel as BM on K.KL_WIJZEBET = BM.BM_CODE left join tbl_betalingsvoorwaarde as BV on K.kl_betalingsvoorwaarde_id = BV.BVW_ID left join tbl_belasting_code as BC on K.kl_belasting_code_id = BC.BEL_ID left join tbl_rekening as R on K.kl_rekening_id = R.REK_ID where K.KL_COD = {customerId}";

        //get data form ETS
        String json = ETSClient.selectQuery(query);

        //check if json is not empty
        if (json == null)
        {
            throw new Exception("Error getting customer from ETS with query: " + query);
        }

        //convert data to customerETS object
        CustomersETS customer = JsonTool.ConvertTo<CustomersETS[]>(json).FirstOrDefault();
        return customer;
    }
}