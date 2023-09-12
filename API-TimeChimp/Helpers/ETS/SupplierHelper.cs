namespace Api.Devion.Helpers.ETS;

public class ETSSupplierHelper : ETSHelper
{
    public ETSSupplierHelper(FirebirdClientETS client) : base(client)
    {
    }

    public string FindSupplierId(string name)
    {
        string query = "SELECT LV_COD FROM LVPX WHERE UPPER(LV_NAM) LIKE @name";
        Dictionary<string, object> parameters = new()
        {
            {"@name", $"%{name.ToUpper()}%" },
        };

        string json = ETSClient.selectQuery(query, parameters) ?? throw new Exception("Error getting artikelnumbers from ETS with query: " + query);

        string supplierId = JsonTool.ConvertTo<List<Dictionary<string, string>>>(json)
            .Select(r => r["LV_COD"])
            .FirstOrDefault() ?? throw new Exception($"No supplier found with ({name}) in the name");

        return supplierId;
    }
}