using Swashbuckle.AspNetCore.SwaggerGen;

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

    public Dictionary<string, object> GetPurchaseOrder(string id)
    {
        //create query
        string query = $"SELECT LVPX.LV_NAM, CSFDPX.* FROM CSFDPX " +
            $"LEFT JOIN CSARTPX ON CSFDPX.FD_ARTNR = CSARTPX.ART_NR " +
            $"LEFT JOIN LVPX ON LVPX.LV_COD = CSARTPX.ART_LEV1 " +
            $"WHERE FD_BONNR = '{id}' AND FD_CODE = 'V'";

        //get data from ETS
        string json = ETSClient.selectQuery(query);

        //check if json is not empty
        if (json == null)
        {
            throw new Exception("Error getting purchase orders from ETS with query: " + query);
        }

        //get all purchase orders from the json
        List<PurchaseOrderDetailETS> purchaseOrders = JsonTool.ConvertTo<List<PurchaseOrderDetailETS>>(json);

        //convert to a dictionary for the web
        Dictionary<string, object> result = new()
        {
            {"bonNummer", purchaseOrders.First().FD_BONNR},
            {"artikels", new List<Dictionary<string, object>>()},
            {"klant", purchaseOrders.First().KLANTNAAM},
            {"project", purchaseOrders.First().FD_PROJ},
            {"subproject", purchaseOrders.First().FD_SUBPROJ}
        };
        foreach (PurchaseOrderDetailETS purchaseOrder in purchaseOrders.Where(p => p.FD_ARTNR != null))
        {
            ((List<Dictionary<string, object>>) result["artikels"]).Add(new()
            {
                {"artikelNummer", purchaseOrder.FD_ARTNR},
                {"omschrijving", purchaseOrder.FD_OMS},
                {"aantal", purchaseOrder.FD_AANTAL.Value},
                {"leverancier", purchaseOrder.LV_NAM}
            });
        }
        return result;
    }
}