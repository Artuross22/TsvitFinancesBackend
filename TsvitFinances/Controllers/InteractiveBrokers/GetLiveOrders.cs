using Brokers.IBKR.Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace TsvitFinances.Controllers.InteractiveBrokers;

[ApiController]
[Route("api/[controller]")]
public class GetLiveOrders : Controller
{
    private readonly IBKRClient _ibkrApiService;
    private readonly ILogger<GetLiveOrders> _logger;

    public GetLiveOrders(IBKRClient ibkrApiService, ILogger<GetLiveOrders> logger)
    {
        _ibkrApiService = ibkrApiService;
        _logger = logger;
    }

    [HttpGet("live-orders")]
    public async Task<ActionResult<object>> Invoke()
    {
        _logger.LogInformation("Live orders request received");

        var result = await _ibkrApiService.GetLiveOrdersAsync();

        if (result.Success)
        {
            _logger.LogInformation("Live orders retrieved successfully");
            return Ok(result.Data);
        }

        _logger.LogError("Failed to retrieve live orders. Error: {Error}", result.Error);
        return BadRequest(result.Error);
    }
}