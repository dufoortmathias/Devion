namespace Api.Devion.Helpers.TimeChimp
{
    public abstract class TimeChimpHelper
    {
        protected WebClient TCClient;

        protected TimeChimpHelper(WebClient client)
        {
            TCClient = client;
        }
    }
}
