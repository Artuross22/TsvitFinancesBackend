using Brokers.IBKR.Client.Configuration;
using Brokers.IBKR.Client.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Brokers.IBKR.Client.Services;

public class IBKRPaperTradingService : IBKRClient
{
    public IBKRPaperTradingService(HttpClient httpClient, IOptions<IBKROptions> options, ILogger<IBKRClient> logger)
        : base(httpClient, options, logger)
    {
    }

    public bool _isPaperAccount(string accountId)
    {
        return accountId.StartsWith("DU");
    }

    public async Task<IBKRResponse<List<OrderResponse>>> PlacePaperOrderAsync(string accountId, OrderRequest orderRequest)
    {
        if (!_isPaperAccount(accountId))
        {
            return new IBKRResponse<List<OrderResponse>>
            {
                Success = false,
                Error = "This method only works with Paper Trading accounts (DU)"
            };
        }

        _logger.LogInformation($"[PAPER TRADING] Placing order on paper account: {accountId}");
        return await PlaceLimitOrderAsync(accountId, orderRequest);
    }

    public async Task<IBKRResponse<PaperTradingBalance>> GetPaperBalanceAsync(string accountId)
    {
        if (!_isPaperAccount(accountId))
        {
            return new IBKRResponse<PaperTradingBalance>
            {
                Success = false,
                Error = "This account is not a Paper Trading account"
            };
        }

        try
        {
            var summaryResult = await GetAccountSummaryAsync(accountId);
            if (summaryResult.Success)
            {
                var balance = new PaperTradingBalance
                {
                    AccountId = accountId,
                    TotalCash = GetValueFromSummary(summaryResult.Data, "TotalCashValue"),
                    NetLiquidation = GetValueFromSummary(summaryResult.Data, "NetLiquidation"),
                    BuyingPower = GetValueFromSummary(summaryResult.Data, "BuyingPower"),
                    Currency = "USD"
                };

                return new IBKRResponse<PaperTradingBalance> { Success = true, Data = balance };
            }
            return new IBKRResponse<PaperTradingBalance> { Success = false, Error = summaryResult.Error };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while retrieving Paper Trading balance");
            return new IBKRResponse<PaperTradingBalance> { Success = false, Error = ex.Message };
        }
    }

    private double GetValueFromSummary(List<AccountSummary> summary, string type)
    {
        var item = summary?.FirstOrDefault(s => s.Type == type);
        return double.TryParse(item?.Amount, out var value) ? value : 0;
    }
}