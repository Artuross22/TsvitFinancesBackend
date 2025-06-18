namespace Brokers.IBKR.Client.Models;

public class AccountsResponse
{
    public List<PaperAccount> Accounts { get; set; }
    public List<string> Aliases { get; set; }
    public string SelectedAccount { get; set; }
}