using System.Text.Json.Serialization;

namespace Brokers.IBKR.Client.Models;

public class AuthStatus
{
    [JsonPropertyName("authenticated")]
    public bool Authenticated { get; set; }

    [JsonPropertyName("connected")]
    public bool Connected { get; set; }

    [JsonPropertyName("competing")]
    public bool Competing { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }
}