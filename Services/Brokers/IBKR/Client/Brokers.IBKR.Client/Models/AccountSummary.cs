namespace Brokers.IBKR.Client.Models;

public class AccountSummary
{
    public string AcctId { get; set; }
    public string Type { get; set; }
    public string Amount { get; set; }
    public string Currency { get; set; }
    public string Value { get; set; }
    public double Severity { get; set; }
}
