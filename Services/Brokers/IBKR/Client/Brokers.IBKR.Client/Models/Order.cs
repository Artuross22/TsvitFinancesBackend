using System.Text.Json.Serialization;

namespace Brokers.IBKR.Client.Models;

public class Order
{
    [JsonPropertyName("orderId")]
    public int OrderId { get; set; }

    [JsonPropertyName("conid")]
    public int ContractId { get; set; }

    [JsonPropertyName("ticker")]
    public string Ticker { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("side")]
    public string Side { get; set; } = string.Empty;

    [JsonPropertyName("remainingQuantity")]
    public decimal RemainingQuantity { get; set; }

    [JsonPropertyName("price")]
    public decimal? Price { get; set; }

    [JsonPropertyName("orderType")]
    public string OrderType { get; set; } = string.Empty;
}