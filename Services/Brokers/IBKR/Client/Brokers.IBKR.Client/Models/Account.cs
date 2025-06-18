using System.Text.Json.Serialization;

namespace Brokers.IBKR.Client.Models;

public class Account
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("accountId")]
    public string AccountId { get; set; } = string.Empty;

    [JsonPropertyName("accountVan")]
    public string? AccountVan { get; set; }

    [JsonPropertyName("accountTitle")]
    public string? AccountTitle { get; set; }

    [JsonPropertyName("displayName")]
    public string? DisplayName { get; set; }

    [JsonPropertyName("accountAlias")]
    public string? AccountAlias { get; set; }

    [JsonPropertyName("accountStatus")]
    public long AccountStatus { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("type")]
    public string? Type { get; set; }
}