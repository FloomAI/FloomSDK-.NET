using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
public class FirePrompt
{
    private readonly string _url;
    private readonly string _apiKey;

    /// <summary>
    /// Creates a new FirePrompt instance, using credentials from Environment Variables.
    /// </summary>
    public FirePrompt()
    {
        _url = Environment.GetEnvironmentVariable("FIRE_ENGINE_URL") ?? "";
        _apiKey = Environment.GetEnvironmentVariable("FIRE_ENGINE_API_KEY") ?? "";

        if (string.IsNullOrEmpty(_url) || string.IsNullOrEmpty(_apiKey))
        {
            throw new InvalidOperationException("FIRE_ENGINE_URL and FIRE_ENGINE_API_KEY environment variables must be set.");
        }
    }

    /// <summary>
    /// Creates a new FirePrompt instance, using provided credentials.
    /// </summary>
    public FirePrompt(string url, string apiKey)
    {
        _url = url;
        _apiKey = apiKey;
    }

    /// <summary>
    /// Fire (Invoke) a pipeline according to its configurations and dynamic parameters you provide
    /// </summary>
    /// <param name="id">Pipeline ID</param>
    /// <param name="input">User input</param>
    /// <param name="variables">Other variables that will compile with both System and User inputs</param>
    public async Task<object> FireAsync(
    string id,
    object input = null,
    Dictionary<string, string> variables = null)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("Api-Key", _apiKey);

            string url = $"{_url}/Fire";

            // Create the object to be sent
            var payload = new
            {
                id = id,
                input = input,
                variables = variables
            };

            // Serialize the object to JSON
            string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
            StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Send the POST request with the JSON content
            HttpResponseMessage response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            string responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }
    }
}

