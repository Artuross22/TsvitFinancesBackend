namespace Brokers.IBKR.Client.Models;

public class PortfolioPosition
{
    public string AcctId { get; set; }
    public int ConId { get; set; }
    public string ContractDesc { get; set; }
    public string AssetClass { get; set; }
    public double Position { get; set; }
    public double MktPrice { get; set; }
    public double MktValue { get; set; }
    public double Currency { get; set; }
    public double AvgCost { get; set; }
    public double UnrealizedPnl { get; set; }
    public double RealizedPnl { get; set; }
}
