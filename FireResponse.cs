public class FireResponse
{
    public string messageId { get; set; } = "";
    public string chatId { get; set; } = "";
    public List<ResponseValue> values { get; set; } = new List<ResponseValue>();
    public long processingTime { get; set; }
    public FireResponseTokenUsage tokenUsage { get; set; }
}

public class ResponseValue
{
    public DataType type { get; set; } = DataType.String;
    public string format { get; set; } = "";
    public string value { get; set; } = ""; //String value
    public string b64 { get; set; } = "";
    public string url { get; set; } = "";
}

public enum DataType
{
    String = 1,
    Image = 2,
    Video = 3,
    Audio = 4
}

public class FireResponseTokenUsage
{
    public int processingTokens { get; set; }
    public int promptTokens { get; set; }
    public int totalTokens { get; set; }
}