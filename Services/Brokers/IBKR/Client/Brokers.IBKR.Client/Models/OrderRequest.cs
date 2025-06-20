namespace Brokers.IBKR.Client.Models;

public class OrderRequest
{
    public int ConId { get; set; }
    public string OrderType { get; set; } = "LMT"; // LMT, MKT, STP, etc.
    public string ListingExchange { get; set; } = "SMART";
    public string OutsideRth { get; set; } = "false";
    public double Price { get; set; }
    public string Side { get; set; } // BUY, SELL
    public string Tif { get; set; } = "DAY"; // DAY, GTC, IOC, FOK
    public string Referrer { get; set; } = "QuickTrade";
    public int Quantity { get; set; }
    public bool UseAdaptive { get; set; } = true;
}