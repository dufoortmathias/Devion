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
        string query = "SELECT * FROM CSFHPX WHERE FH_GEMAILD = 0 AND FH_WEBSHOP = 0 AND FH_AFGEDRUKT = 'F' AND FH_AFGEWERKT = 'N' AND FH_CODE = 'V'";

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

    public PurchaseOrderHeaderETS GetPurchaseOrderHeader(string id)
    {
        //create query
        string query = $"SELECT * FROM CSFHPX WHERE FH_BONNR = @id AND FH_CODE = 'V'";
        Dictionary<string, object> parameters = new()
        {
            {"@id",  id}
        };

        //get data from ETS
        string json = ETSClient.selectQuery(query, parameters);

        //check if json is not empty
        if (json == null)
        {
            throw new Exception("Error getting purchase order header from ETS with query: " + query);
        }

        //get all purchase orders from the json
        PurchaseOrderHeaderETS purchaseOrder = JsonTool.ConvertTo<List<PurchaseOrderHeaderETS>>(json).First();

        return purchaseOrder;
    }

    //get all purchase details for specific purchase order
    public List<PurchaseOrderDetailETS> GetPurchaseOrderDetails(string id)
    {
        //create query
        string query = @"
SELECT LVPX.LV_NAM, LVPX.LV_COD, CSARTPX.ART_LEVREF, CSARTPX.ART_OMS, VCC.*
FROM (
    SELECT CSFDPX.FD_ARTNR, CSFDPX.FD_BONNR,  SUM(CSFDPX.FD_AANTAL) AS TOTAAL_AANTAL
    FROM CSFDPX 
    WHERE FD_BONNR = @ID AND FD_CODE = 'V'
    GROUP BY CSFDPX.FD_ARTNR, CSFDPX.FD_BONNR
) VCC
LEFT JOIN CSARTPX ON VCC.FD_ARTNR = CSARTPX.ART_NR
LEFT JOIN LVPX ON LVPX.LV_COD = CSARTPX.ART_LEV1
";
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
    public FileContentResult CreateCSVFile(List<PurchaseOrderDetailETS> purchaseOrders, string supplier, string seperator)
    {
        var csv = new StringBuilder();
        foreach (PurchaseOrderDetailETS purchaseOrder in purchaseOrders)
        {
            // Add data to the CSV file
            var first = purchaseOrder.FD_KLANTREFERENTIE;
            var second = purchaseOrder.TOTAAL_AANTAL ?? throw new Exception($"PurchaseOrder {purchaseOrder.FD_BONNR} in ETS has no FD_AANTAL");
            var newLine = $"{first}{seperator}{second}";
            csv.AppendLine(newLine);
        }

        byte[] byteData = Encoding.ASCII.GetBytes(csv.ToString());
        return new FileContentResult(byteData, "text/csv")
        {
            FileDownloadName = $"{purchaseOrders.FirstOrDefault()?.FD_BONNR}_{supplier}.csv"
        };
    }

    //creates XML file with all order information for given purchaseOrders, and returns file information
    public FileContentResult CreateFileCebeo(List<Dictionary<string, object>> orderLines, string bestelbon, string supplier, ConfigurationManager config)
    {
        CebeoXML cebeoXML = CebeoXML.CreateOrderRequest(orderLines, config);

        byte[] byteData = Encoding.ASCII.GetBytes(cebeoXML.GetXML());
        return new FileContentResult(byteData, "text/xml")
        {
            FileDownloadName = $"{bestelbon}_{supplier}.xml"
        };
    }
}