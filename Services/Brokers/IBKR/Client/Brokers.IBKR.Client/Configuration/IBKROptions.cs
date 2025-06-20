namespace Brokers.IBKR.Client.Configuration;

public class IBKROptions
{
    public const string SectionName = "IBKR";
    public string BaseUrl { get; set; } = "https://localhost:5000";
    public int TimeoutMinutes { get; set; } = 5;
    public bool IgnoreSSLErrors { get; set; } = true;
    public string UserAgent { get; set; } = "IBKR.Client/1.0";
    public bool IsPaperTrading { get; set; } = true;
    public string PaperTradingAccountPrefix { get; set; } = "DU";
}