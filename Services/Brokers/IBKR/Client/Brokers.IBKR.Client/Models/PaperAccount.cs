namespace Brokers.IBKR.Client.Models;

public class AccountInfo
{
    public required string AccountId { get; set; }
    public string Alias { get; set; } = string.Empty;
    public bool IsPaperAccount { get; set; }
}
