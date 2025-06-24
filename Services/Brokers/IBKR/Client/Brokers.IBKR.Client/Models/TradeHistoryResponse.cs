using System.Text.Json.Serialization;

namespace Brokers.IBKR.Client.Models;

public class TradeHistoryResponse
{
    [JsonPropertyName("trades")]
    public List<_Trade> Trades { get; set; } = new List<_Trade>();

    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    [JsonPropertyName("accountId")]
    public string AccountId { get; set; } = string.Empty;

    [JsonPropertyName("daysRequested")]
    public int DaysRequested { get; set; }

    [JsonPropertyName("requestTime")]
    public DateTime RequestTime { get; set; }

    public class _Trade
    {
        [JsonPropertyName("execution_id")]
        public string ExecutionId { get; set; } = string.Empty;

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; } = string.Empty;

        [JsonPropertyName("side")]
        public string Side { get; set; } = string.Empty;

        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }

        [JsonPropertyName("execution_time")]
        public DateTime ExecutionTime { get; set; }

        [JsonPropertyName("order_id")]
        public string OrderId { get; set; } = string.Empty;

        [JsonPropertyName("exchange")]
        public string Exchange { get; set; } = string.Empty;

        [JsonPropertyName("commission")]
        public decimal Commission { get; set; }

        [JsonPropertyName("currency")]
        public string Currency { get; set; } = string.Empty;

        [JsonPropertyName("secType")]
        public string SecType { get; set; } = string.Empty;
    }
}