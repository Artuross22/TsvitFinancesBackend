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


//public class MarketData
//{
//    public int ConId { get; set; }
//    public string Symbol { get; set; }
//    public double? Last { get; set; }
//    public double? Bid { get; set; }
//    public double? Ask { get; set; }
//    public long? BidSize { get; set; }
//    public long? AskSize { get; set; }
//    public double? High { get; set; }
//    public double? Low { get; set; }
//    public long? Volume { get; set; }
//}