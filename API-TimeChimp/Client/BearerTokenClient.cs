namespace Api.Devion.Client;

public interface IBearerTokenHttpClient
{
    Task<string> GetAsync(string endpoint);
    Task<string> PostAsync(string endpoint, string jsonPayload);
    Task<string> PutAsync(string endpoint, string jsonPayload);
}

public class BearerTokenHttpClient : IBearerTokenHttpClient
{

    private const string bearerToken = "3Hr14yu7DrW4R7YcRfSQDTjBldTpvRuqvzoUG60uN_Sqyl2dlBZakWwyIfZsH4GKeSPAkj1sp8y6zKSJhgQ8pXhAFukK9VB1AjcU97NVkqj8LO1nGof_9dy4u4Ui4EBgnt3Nmyu9tU-ia0cYcwqZJnlMDP-YunXu9hH-230PnlklEy-nHOZ7a7bORvJ0zYMM_U961cfJNeAXH39kFIDfOj9KtnGGZbgwfvDfm6KapW-uoT7ehUN1lLLVhXSTlQO1SNjRkDN15ZRLA9veYydybmizGIQtMVIxvZ726G3GCGpj4nvx";
    private const string baseUrl = "https://api.timechimp.com/v1/";
    private readonly HttpClient _httpClient;

    public BearerTokenHttpClient()
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
