namespace Brokers.IBKR.Client.Models;

public class MarketData
{
    public int ConId { get; set; }
    public string Symbol { get; set; }
    public double? Last { get; set; }
    public double? Bid { get; set; }
    public double? Ask { get; set; }
    public long? BidSize { get; set; }
    public long? AskSize { get; set; }
    public double? High { get; set; }
    public double? Low { get; set; }
    public long? Volume { get; set; }
}