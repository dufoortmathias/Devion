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

    // Create a new instance of HttpClient that is configured with the base address of the API and the bearer token
    public BearerTokenHttpClient(String url, String token)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(url)
        };

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    //get async
    public async Task<string> GetAsync(string endpoint)
    {
        // Send the request to the API endpoint with response
        HttpResponseMessage response = await _httpClient.GetAsync(endpoint);

        //check if statuscode is success
        if (response.IsSuccessStatusCode)
        {
            // Return the response body
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            // Handle error response if needed
            return null;
        }
    }

    //post async
    public async Task<string> PostAsync(string endpoint, string jsonPayload)
    {
        // Create a StringContent object with the JSON payload
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        //send the request to the API endpoint with response
        HttpResponseMessage response = await _httpClient.PostAsync(endpoint, content);

        //check if statuscode is success
        if (response.IsSuccessStatusCode)
        {
            // Return the response body
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            // Handle error response if needed
            return null;
        }
    }

    //put async
    public async Task<string> PutAsync(string endpoint, string jsonPayload)
    {
        // Create a StringContent object with the JSON payload
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        //send the request to the API endpoint with response
        HttpResponseMessage response = await _httpClient.PutAsync(endpoint, content);

        //check if statuscode is success
        if (response.IsSuccessStatusCode)
        {
            // Return the response body
            return await response.Content.ReadAsStringAsync();
        }
        else
        {
            // Handle error response if needed
            return null;
        }
    }
}
