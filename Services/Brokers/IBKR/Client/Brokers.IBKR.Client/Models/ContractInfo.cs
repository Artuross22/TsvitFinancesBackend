namespace Brokers.IBKR.Client.Models;

public class ContractInfo
{
    public int ConId { get; set; }
    public string Symbol { get; set; }
    public string SecType { get; set; }
    public string ListingExchange { get; set; }
    public string SmartAvailable { get; set; }
    public string ContractMonth { get; set; }
}