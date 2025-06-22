using Brokers.IBKR.Client.Models;
using Brokers.IBKR.Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace TsvitFinances.Controllers.InteractiveBrokers;

public class GetTradesHistory : Controller
{
    private readonly IBKRClient _ibkrApiService;
    private readonly ILogger<GetTradesHistory> _logger;

    public GetTradesHistory(IBKRClient ibkrApiService, ILogger<GetTradesHistory> logger)
    {
        _ibkrApiService = ibkrApiService;
        _logger = logger;
    }


    [HttpGet("{accountId}/trades")]
    public async Task<ActionResult<TradeHistoryResponse>> Invoke(string accountId, [FromQuery] int days = 7)
    {
        _logger.LogInformation("_Trade history request received for account: {AccountId}, days: {Days}", accountId, days);

        if (string.IsNullOrWhiteSpace(accountId))
        {
            _logger.LogWarning("Invalid account ID for trade history");
            return BadRequest("Account ID is required");
        }

        if (days <= 0 || days > 365)
        {
            _logger.LogWarning("Invalid days parameter: {Days}", days);
            return BadRequest("Days must be between 1 and 365");
        }

        var result = await _ibkrApiService.GetTradesAsync(accountId, days);

        if (result.Success)
        {
            _logger.LogInformation("_Trade history retrieved successfully for account: {AccountId}", accountId);
            return Ok(result.Data);
        }

        _logger.LogError("Failed to retrieve trade history for account: {AccountId}. Error: {Error}", accountId, result.Error);
        return BadRequest(result.Error);
    }
}