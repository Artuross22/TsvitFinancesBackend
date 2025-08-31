namespace Brokers.IBKR.Client.Configuration;

public class IBKROptions
{
    public required string BaseUrl { get; set; }
    public required int TimeoutMinutes { get; set; }
    public required bool IgnoreSSLErrors { get; set; }
    public required string UserAgent { get; set; }
    public required bool IsPaperTrading { get; set; }
    public required string PaperTradingAccountPrefix { get; set; }
}