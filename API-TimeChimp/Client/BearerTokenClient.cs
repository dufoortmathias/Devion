namespace Api.Devion.Client;

public interface IBearerTokenHttpClient
{
    String GetAsync(string endpoint);
    String PostAsync(string endpoint, string jsonPayload);
    String PutAsync(string endpoint, string jsonPayload);
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
    public String GetAsync(string endpoint)
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
            throw new Exception($"GET error with endpoint: {_httpClient.BaseAddress + endpoint}\n{response.RequestMessage}");
        }
    }

    //post async
    public String PostAsync(string endpoint, string jsonPayload)
    {
        // Create a StringContent object with the JSON payload
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

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
            throw new Exception($"POST error with endpoint: {_httpClient.BaseAddress + endpoint}\n{response.RequestMessage}");
        }
    }

    //put async
    public String PutAsync(string endpoint, string jsonPayload)
    {
        // Create a StringContent object with the JSON payload
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

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
            throw new Exception($"PUT error with endpoint: {_httpClient.BaseAddress + endpoint}\n{response.RequestMessage}");
        }
    }
}
