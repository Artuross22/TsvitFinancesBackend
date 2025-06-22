using Brokers.IBKR.Client.Models;
using Brokers.IBKR.Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace TsvitFinances.Controllers.InteractiveBrokers;

public class GetMarketData : Controller
{
    private readonly IBKRClient _ibkrApiService;
    private readonly ILogger<GetMarketData> _logger;

    public GetMarketData(IBKRClient ibkrApiService, ILogger<GetMarketData> logger)
    {
        _ibkrApiService = ibkrApiService;
        _logger = logger;
    }

    [HttpGet("snapshot")]
    public async Task<ActionResult<List<MarketData>>> Invoke([FromQuery] string conIds, [FromQuery] string fields = "31,84,86")
    {
        _logger.LogInformation("Market data request received for contracts: {ConIds} with fields: {Fields}", conIds, fields);

        if (string.IsNullOrWhiteSpace(conIds))
        {
            _logger.LogWarning("Invalid contract IDs provided for market data");
            return BadRequest("Contract IDs are required");
        }

        var result = await _ibkrApiService.GetMarketDataAsync(conIds, fields);

        if (result.Success)
        {
            _logger.LogInformation("Market data retrieved successfully for contracts: {ConIds}", conIds);
            return Ok(result.Data);
        }

        _logger.LogError("Failed to retrieve market data for contracts: {ConIds}. Error: {Error}", conIds, result.Error);
        return BadRequest(result.Error);
    }
}
