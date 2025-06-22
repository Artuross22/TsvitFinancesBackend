using Brokers.IBKR.Client.Models;
using Brokers.IBKR.Client.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TsvitFinances.Controllers.InteractiveBrokers;

[ApiController]
[Route("api/[controller]")]
public class PlaceMarketOrder : Controller
{
    private readonly IBKRClient _ibkrService;
    private readonly ILogger<PlaceMarketOrder> _logger;

    public PlaceMarketOrder(IBKRClient ibkrApiService, ILogger<PlaceMarketOrder> logger)
    {
        _ibkrService = ibkrApiService;
        _logger = logger;
    }

    [HttpPost("market-order")]
    public async Task<ActionResult<List<OrderResponse>>> Invoke([FromBody] PlaceMarketOrderRequest request)
    {
        _logger.LogInformation("Market order request received: {Side} {Quantity} for contract {ConId}, account: {AccountId}",
            request.Side, request.Quantity, request.ConId, request.AccountId);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid market order request received");
            return BadRequest(ModelState);
        }

        var result = await _ibkrService.PlaceMarketOrderAsync(request.AccountId, request.ConId, request.Side, request.Quantity);

        if (result.Success)
        {
            _logger.LogInformation("Market order placed successfully for account: {AccountId}", request.AccountId);
            return Ok(result.Data);
        }

        _logger.LogError("Failed to place market order for account: {AccountId}. Error: {Error}", request.AccountId, result.Error);
        return BadRequest(result.Error);
    }

    public class PlaceMarketOrderRequest
    {
        [Required]
        public string AccountId { get; set; }

        [Required]
        public int ConId { get; set; }

        [Required]
        [RegularExpression("^(BUY|SELL)$", ErrorMessage = "Side must be BUY or SELL")]
        public string Side { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public int Quantity { get; set; }
    }
}
