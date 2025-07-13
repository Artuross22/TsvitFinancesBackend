using Brokers.IBKR.Client.Models;
using Microsoft.Extensions.Logging;

namespace Brokers.IBKR.Client.Services;

public class PaperTradingDemoService
{
    private readonly IBKRPaperTradingService _ibkrService;
    private readonly ILogger<PaperTradingDemoService> _logger;

    public PaperTradingDemoService(IBKRPaperTradingService ibkrService, ILogger<PaperTradingDemoService> logger)
    {
        _ibkrService = ibkrService;
        _logger = logger;
    }

    public async Task<bool> DemoTradingScenarioAsync()
    {
        try
        {
            _logger.LogInformation("Start of demo trading");

            var accountsResult = await _ibkrService.GetPaperAccountsAsync();
            if (!accountsResult.Success || accountsResult.Data?.Count == 0)
            {
                _logger.LogError("No Paper Trading accounts found");
                return false;
            }

            _logger.LogInformation("Demo trading completed successfully!");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during demo trading");
            return false;
        }
    }
}