namespace Brokers.IBKR.Client.Models;

public class TradeHistoryResponse
{
    public IEnumerable<_Trade> Trades { get; set; }
    public int TotalCount { get; set; }
    public string AccountId { get; set; }
    public int DaysRequested { get; set; }
    public DateTime RequestTime { get; set; }

    public class _Trade
    {
        public string ExecutionId { get; set; }
        public string Symbol { get; set; }
        public string Side { get; set; } // BUY/SELL
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime ExecutionTime { get; set; }
        public string OrderId { get; set; }
        public string Exchange { get; set; }
        public decimal Commission { get; set; }
        public string Currency { get; set; }
        public string SecType { get; set; } // STK, OPT, FUT, etc.
    }

}