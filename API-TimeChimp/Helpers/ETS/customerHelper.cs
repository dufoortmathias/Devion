using Newtonsoft.Json;
using System.Linq;

namespace Api.Devion.Helpers.ETS;

public static class ETSCustomerHelper
{
    public static String[] GetCustomerIdsChangedAfter(DateTime date)
    {
        FirebirdClientETS client = new FirebirdClientETS();
        string query = $"SELECT KL_COD FROM KLPX WHERE DATE_CHANGED BETWEEN '{date.ToString("MM/dd/yyyy HH:mm")}' AND '{DateTime.Now.ToString("MM/dd/yyyy HH:mm")}'";
        string json = client.selectQuery(query);
        String[] ids = JsonTool.ConvertTo<CustomersETS[]>(json)
            .Select(customer => customer.KL_COD)
            .ToArray();
        return ids;
    }

    public static List<CustomersETS> GetCustomers()
    {
        FirebirdClientETS client = new FirebirdClientETS();
        string query = "select * from KLPX as K left join betaalmiddel as BM on K.KL_WIJZEBET = BM.BM_CODE left join tbl_betalingsvoorwaarde as BV on K.kl_betalingsvoorwaarde_id = BV.BVW_ID left join tbl_belasting_code as BC on K.kl_belasting_code_id = BC.BEL_ID left join tbl_rekening as R on K.kl_rekening_id = R.REK_ID; ";
        string json = client.selectQuery(query);
        List<CustomersETS> customers = JsonTool.ConvertTo<List<CustomersETS>>(json);
        return customers;
    }

    public static CustomersETS GetCustomer(String customerId)
    {
        FirebirdClientETS client = new();
        string query = $"select * from KLPX as K left join betaalmiddel as BM on K.KL_WIJZEBET = BM.BM_CODE left join tbl_betalingsvoorwaarde as BV on K.kl_betalingsvoorwaarde_id = BV.BVW_ID left join tbl_belasting_code as BC on K.kl_belasting_code_id = BC.BEL_ID left join tbl_rekening as R on K.kl_rekening_id = R.REK_ID where K.KL_COD = {customerId}";
        String json = client.selectQuery(query);
        CustomersETS customer = JsonTool.ConvertTo<CustomersETS[]>(json).FirstOrDefault();
        return customer;
    }
}