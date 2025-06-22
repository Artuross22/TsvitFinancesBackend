using Brokers.IBKR.Client.Models;
using Brokers.IBKR.Client.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TsvitFinances.Controllers.InteractiveBrokers;

[ApiController]
[Route("api/[controller]")]
public class PlaceLimitOrder : Controller
{
    private readonly IBKRClient _ibkrService;
    private readonly ILogger<PlaceLimitOrder> _logger;

    public PlaceLimitOrder(IBKRClient ibkrApiService, ILogger<PlaceLimitOrder> logger)
    {
        _ibkrService = ibkrApiService;
        _logger = logger;
    }

    [HttpPost("limit-order")]
    public async Task<ActionResult<List<OrderResponse>>> Invoke([FromBody] PlaceLimitOrderRequest request)
    {
        _logger.LogInformation("Limit order request received: {Side} {Quantity} at {Price} for account: {AccountId}",
            request.Side, request.Quantity, request.Price, request.AccountId);

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid limit order request received");
            return BadRequest(ModelState);
        }

        var orderRequest = new OrderRequest
        {
            ConId = request.ConId,
            OrderType = "LMT",
            Side = request.Side,
            Quantity = request.Quantity,
            Price = request.Price,
            Tif = request.Tif ?? "DAY",
            ListingExchange = request.Exchange ?? "SMART"
        };

        var result = await _ibkrService.PlaceLimitOrderAsync(request.AccountId, orderRequest);

        if (result.Success)
        {
            _logger.LogInformation("Limit order placed successfully for account: {AccountId}", request.AccountId);
            return Ok(result.Data);
        }

        _logger.LogError("Failed to place limit order for account: {AccountId}. Error: {Error}", request.AccountId, result.Error);
        return BadRequest(result.Error);
    }

    public class PlaceLimitOrderRequest
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

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public double Price { get; set; }

    public string Tif { get; set; } = "DAY";
    public string Exchange { get; set; } = "SMART";
}
}