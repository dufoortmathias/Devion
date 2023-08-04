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
    private const string bearerTokenMetabil = "k1ddaDjfd5-MFcFka5uOOFAvVv0KY4KVrRLQA5ScaH8y6ROXUrX_TtGUrTDKXFkH30Ug0bzXjgPZjceOiGckLv8R274S4XuCf3FjZWUSL5CAC3spR3ow6GvteL3LsSCBpGmRWGA87PhX9AEThion2C1TIMTGvM7k-VAlGsABaWv0uaNs-g-SH2jMjNnXQKlC1fn1SkpLHKZBCMQzznCnqktdTNrxTkB-95gzFJgrrYMpKQVgErp3piT87YpNjVLfY9TuWOl3eqfZ5u_aD9t9u5hwLtSkY19jUsdRjUPRhm8Dm0M8";
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
