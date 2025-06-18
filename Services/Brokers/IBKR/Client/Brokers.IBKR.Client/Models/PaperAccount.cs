namespace Brokers.IBKR.Client.Models;

public class PaperAccount
{
    public string AccountId { get; set; }
    public string AccountVan { get; set; }
    public string AccountTitle { get; set; }
    public string DisplayName { get; set; }
    public string AccountAlias { get; set; }
    public string AccountStatus { get; set; }
    public string Currency { get; set; }
    public string Type { get; set; }
    public bool TradingType { get; set; }
    public List<string> Faclient { get; set; }
    public bool ClearingStatus { get; set; }
}
