using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class FirePrompt
{
    private readonly string _fireEngineUrl;
    private readonly string _apiKey;

    // Constructor with environment variable-based initialization
    public FirePrompt()
    {
        _fireEngineUrl = Environment.GetEnvironmentVariable("FIRE_ENGINE_URL") ?? "";
        _apiKey = Environment.GetEnvironmentVariable("FIRE_ENGINE_API_KEY") ?? "";

        if (string.IsNullOrEmpty(_fireEngineUrl) || string.IsNullOrEmpty(_apiKey))
        {
            throw new InvalidOperationException("FIRE_ENGINE_URL and FIRE_ENGINE_API_KEY environment variables must be set.");
        }
    }


    // Explicit constructor to pass fireEngineUrl and apiKey as arguments
    public FirePrompt(string fireEngineUrl, string apiKey)
    {
        _fireEngineUrl = fireEngineUrl;
        _apiKey = apiKey;
    }

    public object Fire(string pipelineId, object input = null, Dictionary<string, object> variables = null)
    {
        // Synchronous method to make the HTTP request
        using (HttpClient httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("Api-Key", _apiKey);

            string url = $"{_fireEngineUrl}/pipelines/fire?pipelineId={pipelineId}";

            if (input != null)
            {
                // You can customize the content based on your requirements here
                // For simplicity, let's assume we send the input as JSON
                string jsonInput = Newtonsoft.Json.JsonConvert.SerializeObject(input);
                StringContent content = new StringContent(jsonInput, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");

                HttpResponseMessage response = httpClient.PostAsync(url, content).Result;
                response.EnsureSuccessStatusCode();

                string responseString = response.Content.ReadAsStringAsync().Result;

                // Here, you can handle different response types based on the content type
                // For simplicity, we will return the response as a string
                return responseString;
            }
            else
            {
                HttpResponseMessage response = httpClient.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();

                string responseString = response.Content.ReadAsStringAsync().Result;

                // Here, you can handle different response types based on the content type
                // For simplicity, we will return the response as a string
                return responseString;
            }
        }
    }

    public async Task<object> FireAsync(string pipelineId, object input = null, Dictionary<string, object> variables = null)
    {
        // Asynchronous method to make the HTTP request
        using (HttpClient httpClient = new HttpClient())
        {
            httpClient.DefaultRequestHeaders.Add("Api-Key", _apiKey);

            string url = $"{_fireEngineUrl}/pipelines/fire?pipelineId={pipelineId}";

            if (input != null)
            {
                // You can customize the content based on your requirements here
                // For simplicity, let's assume we send the input as JSON
                string jsonInput = Newtonsoft.Json.JsonConvert.SerializeObject(input);
                StringContent content = new StringContent(jsonInput, Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Add("Content-Type", "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();

                string responseString = await response.Content.ReadAsStringAsync();

                // Here, you can handle different response types based on the content type
                // For simplicity, we will return the response as a string
                return responseString;
            }
            else
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                string responseString = await response.Content.ReadAsStringAsync();

                // Here, you can handle different response types based on the content type
                // For simplicity, we will return the response as a string
                return responseString;
            }
        }
    }
}
