using Newtonsoft.Json;

namespace Brokers.IBKR.Client.Models;

public class TradeHistoryResponse
{
    [JsonProperty("trades")]
    public List<_Trade> Trades { get; set; } = new List<_Trade>();

    [JsonProperty("totalCount")]
    public int TotalCount { get; set; }

    [JsonProperty("accountId")]
    public string AccountId { get; set; } = string.Empty;

    [JsonProperty("daysRequested")]
    public int DaysRequested { get; set; }

    [JsonProperty("requestTime")]
    public DateTime RequestTime { get; set; }

    public class _Trade
    {
        [JsonProperty("execution_id")]
        public string ExecutionId { get; set; } = string.Empty;

        [JsonProperty("symbol")]
        public string Symbol { get; set; } = string.Empty;

        [JsonProperty("side")]
        public string Side { get; set; } = string.Empty;

        [JsonProperty("quantity")]
        public decimal Quantity { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("execution_time")]
        public DateTime ExecutionTime { get; set; }

        [JsonProperty("order_id")]
        public string OrderId { get; set; } = string.Empty;

        [JsonProperty("exchange")]
        public string Exchange { get; set; } = string.Empty;

        [JsonProperty("commission")]
        public decimal Commission { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; } = string.Empty;

        [JsonProperty("secType")]
        public string SecType { get; set; } = string.Empty;
    }
}