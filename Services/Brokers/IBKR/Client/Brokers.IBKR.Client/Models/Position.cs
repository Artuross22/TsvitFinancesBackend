using System.Text.Json.Serialization;

namespace Brokers.IBKR.Client.Models;

public class Position
{
    [JsonPropertyName("acctId")]
    public string? AcctId { get; set; }

    [JsonPropertyName("conid")]
    public int Conid { get; set; }

    [JsonPropertyName("contractDesc")]
    public string? ContractDesc { get; set; }

    [JsonPropertyName("position")]
    public double PositionSize { get; set; }

    [JsonPropertyName("mktPrice")]
    public double MktPrice { get; set; }

    [JsonPropertyName("mktValue")]
    public double MktValue { get; set; }

    [JsonPropertyName("currency")]
    public string? Currency { get; set; }

    [JsonPropertyName("avgCost")]
    public double AvgCost { get; set; }

    [JsonPropertyName("avgPrice")]
    public double AvgPrice { get; set; }

    [JsonPropertyName("realizedPnl")]
    public double RealizedPnl { get; set; }

    [JsonPropertyName("unrealizedPnl")]
    public double UnrealizedPnl { get; set; }
}