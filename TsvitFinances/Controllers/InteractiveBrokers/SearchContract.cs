using Brokers.IBKR.Client.Models;
using Brokers.IBKR.Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace TsvitFinances.Controllers.InteractiveBrokers;

[ApiController]
[Route("api/[controller]")]
public class SearchContract : Controller
{
    private readonly IBKRClient _ibkrService;
    private readonly ILogger<SearchContract> _logger;

    public SearchContract(IBKRClient ibkrApiService, ILogger<SearchContract> logger)
    {
        _ibkrService = ibkrApiService;
        _logger = logger;
    }

    [HttpGet("search/{symbol}")]
    public async Task<ActionResult<List<ContractInfo>>> Invoke(string symbol)
    {
        _logger.LogInformation("Contract search request received for symbol: {Symbol}", symbol);

        if (string.IsNullOrWhiteSpace(symbol))
        {
            _logger.LogWarning("Invalid symbol provided for contract search");
            return BadRequest("Symbol is required");
        }

        var result = await _ibkrService.SearchContractAsync(symbol);

        if (result.Success)
        {
            _logger.LogInformation("Contract search completed successfully for symbol: {Symbol}, found: {Count}",
                symbol, result.Data?.Count ?? 0);
            return Ok(result.Data);
        }

        _logger.LogError("Failed to search contracts for symbol: {Symbol}. Error: {Error}", symbol, result.Error);
        return BadRequest(result.Error);
    }
}
