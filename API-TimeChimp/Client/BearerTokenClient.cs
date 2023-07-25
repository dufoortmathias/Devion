namespace Api.Devion.Client;

public interface IBearerTokenHttpClient
{
    Task<string> GetAsync(string endpoint);
    Task<string> PostAsync(string endpoint, string jsonPayload);
    Task<string> PutAsync(string endpoint, string jsonPayload);
}

public class BearerTokenHttpClient : IBearerTokenHttpClient
{
    private readonly HttpClient _httpClient;

    public BearerTokenHttpClient(string baseUrl, string bearerToken)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(baseUrl)
        };

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
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
