namespace Brokers.IBKR.Client.Models;

public class PaperOrderRequest
{
    public string AccountId { get; set; }
    public int ConId { get; set; }
    public string OrderType { get; set; } = "LMT";
    public string Side { get; set; }
    public int Quantity { get; set; }
    public double Price { get; set; }
    public string Tif { get; set; } = "DAY";
}
