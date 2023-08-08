namespace Api.Devion.Helpers.TimeChimp
{
    public abstract class TimeChimpHelper
    {
        protected BearerTokenHttpClient TCClient;

        protected TimeChimpHelper(BearerTokenHttpClient client)
        {
            TCClient = client;
        }
    }
}
