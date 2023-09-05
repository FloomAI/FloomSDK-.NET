public class FloomRequest
{
    public string pipelineId { get; set; } //Pipeline ID
    public string chatId { get; set; } = ""; //Chat ID
    public string input { get; set; } = "";  //User Input [Should be binary also]
    public Dictionary<string, string> variables { get; set; } //Vars
    public DataTransferType dataTransfer { get; set; }
}

public enum DataTransferType
{
    Base64 = 1,
    //Url = 2
}