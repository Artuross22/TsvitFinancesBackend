namespace Brokers.IBKR.Client.Models;

public class PaperTradingBalance
{
    public string AccountId { get; set; }
    public double TotalCash { get; set; }
    public double NetLiquidation { get; set; }
    public double BuyingPower { get; set; }
    public string Currency { get; set; }
    public DateTime LastUpdate { get; set; } = DateTime.Now;
}
