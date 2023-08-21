namespace Api.Devion.Helpers.ETS;

public class ETSPurchaseOrderHelper : ETSHelper
{
    public ETSPurchaseOrderHelper(FirebirdClientETS FBClient) : base(FBClient)
    {
    }

    public List<PurchaseOrderHeaderETS> GetOpenPurchaseOrders()
    {
        //create query
        string query = $"SELECT * FROM CSFHPX WHERE FH_AFGEWERKT = 'N' AND FH_CODE = 'V'";

        //get data from ETS
        string json = ETSClient.selectQuery(query);

        //check if json is not empty
        if (json == null)
        {
            throw new Exception("Error getting purchase orders from ETS with query: " + query);
        }

        //get all purchase orders from the json
        return JsonTool.ConvertTo<List<PurchaseOrderHeaderETS>>(json);
    }
}