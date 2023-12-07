namespace Api.Devion.Helpers.ETS;

public class ETSProjVoortgangHelper : ETSHelper
{
    public ETSProjVoortgangHelper(FirebirdClientETS FBClient) : base(FBClient)
    {
    }

    public string GetProjectenVoortgang()
    {
        string query = "select * from view_projecten_voortgang";
        string jsonString = ETSClient.selectQuery(query);

        return jsonString;
    }
}
