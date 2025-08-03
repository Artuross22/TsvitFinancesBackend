using Brokers.IBKR.Client.Models;
using Brokers.IBKR.Client.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TsvitFinances.Controllers.InteractiveBrokers;

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

    [HttpGet("{userId}")]
    public async Task<ActionResult> Invoke(string userId)
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