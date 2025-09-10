using Newtonsoft.Json;

namespace Brokers.IBKR.Client.Models;

public class AccountsResponse
{
    [JsonProperty("accounts")]
    public List<string> Accounts { get; set; } = new();

    [JsonProperty("aliases")]
    public Dictionary<string, string> Aliases { get; set; } = new();
}