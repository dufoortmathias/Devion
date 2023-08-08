namespace Api.Devion.Helpers.ETS
{
    public abstract class ETSHelper
    {
        protected FirebirdClientETS ETSClient;

        protected ETSHelper(FirebirdClientETS client)
        {
            ETSClient = client;
        }
    }
}
