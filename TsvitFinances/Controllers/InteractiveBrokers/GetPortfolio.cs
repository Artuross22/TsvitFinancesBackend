using Brokers.IBKR.Client.Models;
using Brokers.IBKR.Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace TsvitFinances.Controllers.InteractiveBrokers;

[ApiController]
[Route("api/[controller]")]
public class PortfolioController : ControllerBase
{
    private readonly IBKRClient _ibkrApiService;
    private readonly ILogger<PortfolioController> _logger;

    public PortfolioController(IBKRClient ibkrApiService, ILogger<PortfolioController> logger)
    {
        _ibkrApiService = ibkrApiService;
        _logger = logger;
    }


    [HttpGet("{accountId}/positions")]
    public async Task<ActionResult<List<PortfolioPosition>>> Invoke(string accountId)
    {
        _logger.LogInformation("Portfolio request received for account: {AccountId}", accountId);

        if (string.IsNullOrWhiteSpace(accountId))
        {
            _logger.LogWarning("Invalid account ID provided");
            return BadRequest("Account ID is required");
        }

        var result = await _ibkrApiService.GetPortfolioAsync(accountId);

        if (result.Success)
        {
            _logger.LogInformation("Portfolio retrieved successfully for account: {AccountId}", accountId);
            return Ok(result.Data);
        }

        _logger.LogError("Failed to retrieve portfolio for account: {AccountId}. Error: {Error}", accountId, result.Error);
        return BadRequest(result.Error);
    }
}