using Newtonsoft.Json;

namespace Brokers.IBKR.Client.Models;

public class AuthStatus
{
    [JsonProperty("authenticated")]
    public bool Authenticated { get; set; }

    [JsonProperty("connected")]
    public bool Connected { get; set; }

    [JsonProperty("competing")]
    public bool Competing { get; set; }

    [JsonProperty("message")]
    public string? Message { get; set; }
}