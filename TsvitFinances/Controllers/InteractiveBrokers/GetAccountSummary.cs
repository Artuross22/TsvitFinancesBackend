using Brokers.IBKR.Client.Models;
using Brokers.IBKR.Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace TsvitFinances.Controllers.InteractiveBrokers;

[ApiController]
[Route("api/[controller]")]
public class GetAccountSummary : ControllerBase
{
    private readonly IBKRClient _ibkrApiService;
    private readonly ILogger<GetAccountSummary> _logger;


    [HttpGet("{accountId}/summary")]
    public async Task<ActionResult<List<AccountSummary>>> Invoke(string accountId)
    {
        _logger.LogInformation("Account summary request received for account: {AccountId}", accountId);

        if (string.IsNullOrWhiteSpace(accountId))
        {
            _logger.LogWarning("Invalid account ID provided for summary");
            return BadRequest("Account ID is required");
        }

        var result = await _ibkrApiService.GetAccountSummaryAsync(accountId);

        if (result.Success)
        {
            _logger.LogInformation("Account summary retrieved successfully for account: {AccountId}", accountId);
            return Ok(result.Data);
        }

        _logger.LogError("Failed to retrieve account summary for account: {AccountId}. Error: {Error}", accountId, result.Error);
        return BadRequest(result.Error);
    }
}