using System.Text.Json.Serialization;

namespace Brokers.IBKR.Client.Models;

public class OrderRequest
{
    [JsonPropertyName("conid")]
    public int Conid { get; set; }

    [JsonPropertyName("orderType")]
    public string OrderType { get; set; } = "MKT";

    [JsonPropertyName("side")]
    public string Side { get; set; } = "BUY";

    [JsonPropertyName("quantity")]
    public double Quantity { get; set; }

    [JsonPropertyName("price")]
    public double? Price { get; set; }

    [JsonPropertyName("tif")]
    public string Tif { get; set; } = "DAY";
}
