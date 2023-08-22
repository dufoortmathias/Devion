using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Api.Devion.Helpers.ETS;

public class ETSPurchaseOrderHelper : ETSHelper
{
    public ETSPurchaseOrderHelper(FirebirdClientETS FBClient) : base(FBClient)
    {
    }

    public List<PurchaseOrderHeaderETS> GetOpenPurchaseOrders()
    {
        //create query
        string query = $"SELECT DISTINCT FH_BONNR FROM CSFHPX WHERE FH_GEMAILD = 0 AND FH_gemaild = 0 AND FH_AFGEWERKT = 'N' AND FH_CODE = 'V'";

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

    public List<PurchaseOrderDetailETS> GetPurchaseOrderDetails(string id)
    {
        //create query
        string query = $"SELECT LVPX.LV_NAM, LVPX.LV_COD, CSFDPX.* FROM CSFDPX " +
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
        
        return purchaseOrders;
    }

    public FileContentResult CreateCSVFile(List<PurchaseOrderDetailETS> purchaseOrders)
    {
        var csv = new StringBuilder();
        foreach (PurchaseOrderDetailETS purchaseOrder in purchaseOrders)
        {
            // Add data to the CSV file
            var first = purchaseOrder.FD_ARTNR;
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

    public FileContentResult CreateFileCebeo(List<PurchaseOrderDetailETS> purchaseOrders, ConfigurationManager config)
    {
        CebeoXML cebeoXML = new(purchaseOrders, config);

        XmlSerializer xsSubmit = new(typeof(CebeoXML));
        var xml = "";

        using (var sww = new StringWriter())
        {
            using (XmlWriter writer = XmlWriter.Create(sww))
            {
                xsSubmit.Serialize(writer, cebeoXML);
                xml = sww.ToString(); // Your XML
            }
        }

        byte[] byteData = Encoding.ASCII.GetBytes(xml);
        return new FileContentResult(byteData, "text/xml")
        {
            FileDownloadName = $"{purchaseOrders.FirstOrDefault()?.FD_BONNR}_{purchaseOrders.FirstOrDefault()?.LV_NAM}.xml"
        };
    }
}