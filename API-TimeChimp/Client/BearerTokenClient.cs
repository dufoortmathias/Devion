namespace Api.Devion.Client;

public interface IBearerTokenHttpClient
{
    string GetAsync(string endpoint);
    string PostAsync(string endpoint, string jsonPayload);
    string PutAsync(string endpoint, string jsonPayload);
}

public class BearerTokenHttpClient : IBearerTokenHttpClient
{
    private readonly HttpClient _httpClient;

    // Create a new instance of HttpClient that is configured with the base address of the API and the bearer token
    public BearerTokenHttpClient(string url, string token)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(url)
        };

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    //get async
    public string GetAsync(string endpoint)
    {
        // Send the request to the API endpoint with response
        HttpResponseMessage response = _httpClient.GetAsync(endpoint).Result;

        //check if statuscode is success
        if (response.IsSuccessStatusCode)
        {
            // Return the response body
            return response.Content.ReadAsStringAsync().Result;
        }
        else
        {
            // Handle error response if needed
            throw new Exception($"GET {response.StatusCode} with endpoint: {_httpClient.BaseAddress + endpoint}");
        }
    }

    //post async
    public string PostAsync(string endpoint, string jsonPayload)
    {
        // Create a StringContent object with the JSON payload
        StringContent content = new(jsonPayload, Encoding.UTF8, "application/json");

        //send the request to the API endpoint with response
        HttpResponseMessage response = _httpClient.PostAsync(endpoint, content).Result;

        //check if statuscode is success
        if (response.IsSuccessStatusCode)
        {
            // Return the response body
            return response.Content.ReadAsStringAsync().Result;
        }
        else
        {
            // Handle error response if needed
            throw new Exception($"POST {response.StatusCode} with endpoint: {_httpClient.BaseAddress + endpoint}");
        }
    }

    //put async
    public string PutAsync(string endpoint, string jsonPayload)
    {
        // Create a StringContent object with the JSON payload
        StringContent content = new(jsonPayload, Encoding.UTF8, "application/json");

        //send the request to the API endpoint with response
        HttpResponseMessage response = _httpClient.PutAsync(endpoint, content).Result;

        //check if statuscode is success
        if (response.IsSuccessStatusCode)
        {
            // Return the response body
            return response.Content.ReadAsStringAsync().Result;
        }
        else
        {
            // Handle error response if needed
            throw new Exception($"PUT {response.StatusCode} with endpoint: {_httpClient.BaseAddress + endpoint}");
        }
    }
}
