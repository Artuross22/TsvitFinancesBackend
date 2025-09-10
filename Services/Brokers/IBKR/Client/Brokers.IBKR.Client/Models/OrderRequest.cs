namespace Brokers.IBKR.Client.Models;

public class OrderRequest
{
    public int Conid { get; set; }
    public string OrderType { get; set; } //"LMT"; // LMT, MKT, STP, etc.
    public double? Price { get; set; }
    public string Side { get; set; } // BUY, SELL
    public string Tif { get; set; } = "DAY"; // DAY, GTC, IOC, FOK
    public int Quantity { get; set; }
}