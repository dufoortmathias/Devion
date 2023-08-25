namespace Api.Devion.Helpers.ETS;

public class ETSPurchaseOrderHelper : ETSHelper
{
    public ETSPurchaseOrderHelper(FirebirdClientETS FBClient) : base(FBClient)
    {
    }

    //get all purchase headers that aren't finished
    public List<PurchaseOrderHeaderETS> GetOpenPurchaseOrders()
    {
        //create query
        string query = $"SELECT * FROM CSFHPX WHERE FH_GEMAILD = 0 AND FH_gemaild = 0 AND FH_AFGEWERKT = 'N' AND FH_CODE = 'V'";

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

    //get all purchase details for specific purchase order
    public List<PurchaseOrderDetailETS> GetPurchaseOrderDetails(string id)
    {
        //create query
        string query = $"SELECT LVPX.LV_NAM, LVPX.LV_COD, CSARTPX.ART_LEVREF, CSFDPX.* FROM CSFDPX " +
            $"LEFT JOIN CSARTPX ON CSFDPX.FD_ARTNR = CSARTPX.ART_NR " +
            $"LEFT JOIN LVPX ON LVPX.LV_COD = CSARTPX.ART_LEV1 " +
            $"WHERE FD_BONNR = @id AND FD_CODE = 'V'";
        Dictionary<string, object> parameters = new()
        {
            {"@id",  id}
        };

        //get data from ETS
        string json = ETSClient.selectQuery(query, parameters);

        //check if json is not empty
        if (json == null)
        {
            throw new Exception("Error getting purchase orders from ETS with query: " + query);
        }

        //get all purchase orders from the json
        List<PurchaseOrderDetailETS> purchaseOrders = JsonTool.ConvertTo<List<PurchaseOrderDetailETS>>(json);
        
        return purchaseOrders;
    }

    //creates CSV file with all order information for given purchaseOrders, and returns file information
    public FileContentResult CreateCSVFile(List<PurchaseOrderDetailETS> purchaseOrders)
    {
        var csv = new StringBuilder();
        foreach (PurchaseOrderDetailETS purchaseOrder in purchaseOrders)
        {
            // Add data to the CSV file
            var first = purchaseOrder.ART_LEVREF;
            var second = purchaseOrder.FD_AANTAL.Value;
            var newLine = string.Format("{0}, {1}", first, second);
            csv.AppendLine(newLine);
        }

        byte[] byteData = Encoding.ASCII.GetBytes(csv.ToString());
        return new FileContentResult(byteData, "text/csv")
        {
            FileDownloadName = $"{purchaseOrders.FirstOrDefault()?.FD_BONNR}_{purchaseOrders.FirstOrDefault()?.LV_NAM}.csv"
        };
    }

    //creates XML file with all order information for given purchaseOrders, and returns file information
    public FileContentResult CreateFileCebeo(List<PurchaseOrderDetailETS> purchaseOrders, ConfigurationManager config)
    {
        CebeoXML cebeoXML = CebeoXML.CreateOrderRequest(purchaseOrders, config);

        byte[] byteData = Encoding.ASCII.GetBytes(cebeoXML.GetXML());
        return new FileContentResult(byteData, "text/xml")
        {
            FileDownloadName = $"{purchaseOrders.FirstOrDefault()?.FD_BONNR}_{purchaseOrders.FirstOrDefault()?.LV_NAM}.xml"
        };
    }
}