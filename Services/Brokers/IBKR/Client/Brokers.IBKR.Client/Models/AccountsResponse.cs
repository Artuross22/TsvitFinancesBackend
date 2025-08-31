using System.Text.Json.Serialization;

namespace Brokers.IBKR.Client.Models;

public class AccountsResponse
{
    [JsonPropertyName("accounts")]
    public List<string> Accounts { get; set; } = new();

    [JsonPropertyName("aliases")]
    public Dictionary<string, string> Aliases { get; set; } = new();
}