using Newtonsoft.Json;

namespace GramGPT.Models;

public class Choice
{
    [JsonProperty("message")]
    public Message Message { get; set; }
}
