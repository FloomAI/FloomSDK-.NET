using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
public class FloomClient
{
    private readonly string _url;
    private readonly string _apiKey;

    /// <summary>
    /// Creates a new FloomClient instance, using provided apiKey.
    /// </summary>
    /// <param name="endpoint">The Floom's endpoint (URL), i.e. http://10.0.0.3:5050</param>
    /// <param name="apiKey"></param>
    public FloomClient(string endpoint, string apiKey)
    {
        _url = endpoint;
        _apiKey = apiKey;
    }

    /// <summary>
    /// Floom (Invoke) a pipeline according to its configurations and dynamic parameters you provide
    /// </summary>
    /// <param name="pipelineId">Pipeline ID</param>
    /// <param name="chatId">Chat ID, an auto-generated OR self-generated identifier that identifies the chat/converation unique session, for memory, history and continuation purposes.</param>
    /// <param name="input">User input</param>
    /// <param name="variables">Other variables that will compile with both System and User inputs</param>
    public async Task<FloomResponse> Run(
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

            string url = $"{_url}/v1/Pipelines/Run";

            // Create the object to be sent
            FloomRequest floomRequest = new FloomRequest()
            {
                pipelineId = pipelineId,
                chatId = chatId,
                input = (string)input,
                variables = variables,
                dataTransfer = dataTransfer
            };

            // Serialize the object to JSON
            string jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(floomRequest);
            StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            // Send the POST request with the JSON content
            HttpResponseMessage response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            string responseString = await response.Content.ReadAsStringAsync();
            FloomResponse floomResponse = JsonConvert.DeserializeObject<FloomResponse>(responseString);

            return floomResponse;
        }
    }
}
