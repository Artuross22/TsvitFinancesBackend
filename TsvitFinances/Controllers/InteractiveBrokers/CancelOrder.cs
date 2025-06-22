using Brokers.IBKR.Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace TsvitFinances.Controllers.InteractiveBrokers;

public class CancelOrder : Controller
{
    private readonly IBKRClient _ibkrApiService;
    private readonly ILogger<CancelOrder> _logger;

    public CancelOrder(IBKRClient ibkrApiService, ILogger<CancelOrder> logger)
    {
        _ibkrApiService = ibkrApiService;
        _logger = logger;
    }

    [HttpDelete("{accountId}/orders/{orderId}")]
    public async Task<ActionResult<object>> Invoke(string accountId, string orderId)
    {
        _logger.LogInformation("Order cancellation request received: {OrderId} for account: {AccountId}", orderId, accountId);

        if (string.IsNullOrWhiteSpace(accountId) || string.IsNullOrWhiteSpace(orderId))
        {
            _logger.LogWarning("Invalid parameters for order cancellation");
            return BadRequest("Account ID and Order ID are required");
        }

        var result = await _ibkrApiService.CancelOrderAsync(accountId, orderId);

        if (result.Success)
        {
            _logger.LogInformation("Order cancelled successfully: {OrderId}", orderId);
            return Ok(result.Data);
        }

        _logger.LogError("Failed to cancel order: {OrderId}. Error: {Error}", orderId, result.Error);
        return BadRequest(result.Error);
    }
}
