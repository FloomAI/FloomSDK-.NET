using Newtonsoft.Json;
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
    /// <param name="pipelineId">Pipeline ID</param>
    /// <param name="chatId">Chat ID, an auto-generated OR self-generated identifier that identifies the chat/converation unique session, for memory, history and continuation purposes.</param>
    /// <param name="input">User input</param>
    /// <param name="variables">Other variables that will compile with both System and User inputs</param>
    public async Task<FireResponse> FireAsync(
        string pipelineId,
        string chatId = "",
        object input = null,
        Dictionary<string, string> variables = null,
        DataTransferType dataTransfer = DataTransferType.Base64
    )
    {
        using (HttpClient httpClient = new HttpClient() { Timeout = new TimeSpan(0,0,160) })
        {
            httpClient.DefaultRequestHeaders.Add("Api-Key", _apiKey);

            string url = $"{_url}/Fire";

            // Create the object to be sent
            FireRequest fireRequest = new FireRequest()
            {
                pipelineId = pipelineId,
                chatId = chatId,
                input = (string)input,
                variables = variables,
                dataTransfer = dataTransfer
            };

            // Serialize the object to JSON
            string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(fireRequest);
            StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Send the POST request with the JSON content
            HttpResponseMessage response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            string responseString = await response.Content.ReadAsStringAsync();
            FireResponse fireResponse = JsonConvert.DeserializeObject<FireResponse>(responseString);

            return fireResponse;
        }
    }
}
