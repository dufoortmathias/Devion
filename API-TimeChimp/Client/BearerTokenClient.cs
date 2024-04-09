namespace Api.Devion.Client;

public interface IWebClient
{
    string GetAsync(string endpoint);
    string PostAsync(string endpoint, string jsonPayload);
    string PutAsync(string endpoint, string jsonPayload);
}

public class WebClient : IWebClient
{
    private readonly HttpClient _httpClient;

    // Create a new instance of HttpClient
    public WebClient()
    {
        _httpClient = new HttpClient();
    }

    // Create a new instance of HttpClient that is configured with the base address of the API and the api token
    public WebClient(string url, string token)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(url)
        };

        _httpClient.DefaultRequestHeaders.Add("api-key", token);
        _httpClient.DefaultRequestHeaders.Add("api-version", "2.0");
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

    public string PatchAsync(string endpoint, string jsonPayload)
    {
        // Create a StringContent object with the JSON payload
        StringContent content = new(jsonPayload, Encoding.UTF8, "application/json");

        //send the request to the API endpoint with response
        HttpResponseMessage response = _httpClient.PatchAsync(endpoint, content).Result;

        //check if statuscode is success
        if (response.IsSuccessStatusCode)
        {
            // Return the response body
            return response.Content.ReadAsStringAsync().Result;
        }
        else
        {
            // Handle error response if needed
            throw new Exception($"PATCH {response.StatusCode} with endpoint: {_httpClient.BaseAddress + endpoint}");
        }
    }
}
