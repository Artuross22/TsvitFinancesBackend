using Brokers.IBKR.Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace TsvitFinances.Controllers.InteractiveBrokers;

[Route("api/[controller]")]
public class GetCureentAccounts : Controller
{
    private readonly IBKRClient _ibkrApiService;
    private readonly ILogger<GetLiveOrders> _logger;

    public GetCureentAccounts(IBKRClient ibkrApiService, ILogger<GetLiveOrders> logger)
    {
        _ibkrApiService = ibkrApiService;
        _logger = logger;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult> Invoke(string userId)
    {
        var result = await _ibkrApiService.GetCurrentAccountAsync();

        if (result.Success)
        {
            _logger.LogInformation("Current account retrieved successfully");
            return Ok(result.Data);
        }

        _logger.LogError("Failed to retrieve current account. Error: {Error}", result.Error);
        return BadRequest(result.Error);
    }
}