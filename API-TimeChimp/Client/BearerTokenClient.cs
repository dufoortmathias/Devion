namespace Api.Devion.Client;

public interface IBearerTokenHttpClient
{
    Task<string> GetAsync(string endpoint);
    Task<string> PostAsync(string endpoint, string jsonPayload);
    Task<string> PutAsync(string endpoint, string jsonPayload);
}

public class BearerTokenHttpClient : IBearerTokenHttpClient
{

    private const string bearerTokenDevion = "sohP2Xh3doeSCigZ2UGm3kECxjsDDb4AsmphuBnJfv35ezR4g2tgtMb8txkELKeDSpa9DVpQD5MG9WgcS-4QJgj0YXw5BzUn92oDEuwft2GiaJfslY-1oUBQ0Mk1gscaMtuf3GgZsvFl8SZ_SFvLA0LoQADmGT0cmbeIMayBuLZija7FZk0sFisAeC6Z97YAkKEq9zWm3N7kEfWnNtp_FxSg5rDmAo5DxGLTRtYCaddQOavq9veveFC4CJ6ew4nAcRlFQkhIN3QOPbBQL4ZB3EA_WD-ZSxJN7SoMiXdzefc-st-h";
    private const string bearerTokenMetabil = "HuBSGtzWIAEXQJUk6JuFr0AgJccsEhodeZlHPpEXdOpQe2XQD2YTXPNd8mRk7N50fwdfRFGrReu6-TLoP6SArOUeIWLOE2wF9wtOf5oUxvY1RrAOeJXmVZOEXncAr_C0VjxwnPWuS7M2CX8_1q_Do9o06zM2_jmFMpaJ32IG1yPoCyyu31zHweIZwNDvE01MvngeCwVSSC13dibVlAG5kkpS2hQ4l3YVteSJct2NJ7ufCtKNGUKk-GCrTd1-l64xmxtMgJW2M4270ZaWngxSoQ0Vv5416EWHiQs8zRW66Q3yqzzc";
    private const string baseUrl = "https://api.timechimp.com/v1/";
    private readonly HttpClient _httpClient;

    public BearerTokenHttpClient()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerTokenDevion);
    }

    public BearerTokenHttpClient(string baseUrl2)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl2)
        };

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerTokenDevion);
    }

    public async Task<string> GetAsync(string endpoint)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            // Handle error response if needed
            return null;
        }
    }

    public async Task<string> PostAsync(string endpoint, string jsonPayload)
    {
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync(endpoint, content);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            // Handle error response if needed
            return null;
        }
    }

    public async Task<string> PutAsync(string endpoint, string jsonPayload)
    {
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PutAsync(endpoint, content);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            // Handle error response if needed
            return null;
        }
    }
}
