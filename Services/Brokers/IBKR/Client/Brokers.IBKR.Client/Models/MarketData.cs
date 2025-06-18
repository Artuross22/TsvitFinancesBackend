using System.Text.Json.Serialization;

namespace Brokers.IBKR.Client.Models;

public class MarketData
{
    [JsonPropertyName("conid")]
    public int Conid { get; set; }

    [JsonPropertyName("31")]
    public string? Last { get; set; }

    [JsonPropertyName("84")]
    public string? Bid { get; set; }

    [JsonPropertyName("85")]
    public string? Ask { get; set; }

    [JsonPropertyName("86")]
    public string? BidSize { get; set; }
}
