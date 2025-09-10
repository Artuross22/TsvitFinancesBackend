using Newtonsoft.Json;

namespace Brokers.IBKR.Client.Models;

public class Order
{
    [JsonProperty("orderId")]
    public int OrderId { get; set; }

    [JsonProperty("conid")]
    public int ContractId { get; set; }

    [JsonProperty("ticker")]
    public string Ticker { get; set; } = string.Empty;

    [JsonProperty("status")]
    public string Status { get; set; } = string.Empty;

    [JsonProperty("side")]
    public string Side { get; set; } = string.Empty;

    [JsonProperty("remainingQuantity")]
    public decimal RemainingQuantity { get; set; }

    [JsonProperty("price")]
    public decimal? Price { get; set; }

    [JsonProperty("orderType")]
    public string OrderType { get; set; } = string.Empty;

    public int Quantity { get; set; }
}