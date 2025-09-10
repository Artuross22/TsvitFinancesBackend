using Newtonsoft.Json;

namespace Brokers.IBKR.Client.Models;

public class Account
{
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [JsonProperty("accountId")]
    public string AccountId { get; set; } = string.Empty;

    [JsonProperty("accountVan")]
    public string? AccountVan { get; set; }

    [JsonProperty("accountTitle")]
    public string? AccountTitle { get; set; }

    [JsonProperty("displayName")]
    public string? DisplayName { get; set; }

    [JsonProperty("accountAlias")]
    public string? AccountAlias { get; set; }

    [JsonProperty("accountStatus")]
    public long AccountStatus { get; set; }

    [JsonProperty("currency")]
    public string? Currency { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }
}