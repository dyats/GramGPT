using Newtonsoft.Json;

namespace GramGPT.Models;

public class ChatGPTRequest
{
    [JsonProperty("model")]
    public string Model { get; set; }

    [JsonProperty("messages")]
    public List<Message> Messages { get; set; }
}