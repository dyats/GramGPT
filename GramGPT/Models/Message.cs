using Newtonsoft.Json;

namespace GramGPT.Models;

public class Message
{
    [JsonProperty("role")]
    public string Role { get; set; }

    [JsonProperty("content")]
    public string Content { get; set; }
}
