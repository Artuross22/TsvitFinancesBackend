using Brokers.IBKR.Client.Models;
using Brokers.IBKR.Client.Services;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TsvitFinances.Controllers.InteractiveBrokers;

[ApiController]
[Route("api/[controller]")]
public class PlaceLimitOrder : Controller
{
    private readonly IBKRClient _ibkrService;
    private readonly ILogger<PlaceLimitOrder> _logger;
    protected readonly MainDb _mainDb;


    public PlaceLimitOrder(IBKRClient ibkrApiService, ILogger<PlaceLimitOrder> logger, MainDb mainDb)
    {
        _ibkrService = ibkrApiService;
        _logger = logger;
        _mainDb = mainDb;
    }

    [HttpPost("limit-order")]
    public async Task<ActionResult<List<OrderResponse>>> Invoke([FromBody] PlaceLimitOrderRequest request)
    {
        _logger.LogInformation("Limit order request received: {Side} {Quantity} at {Price} for account: {AccountId}",
            request.Side, request.Quantity, request.Price, request.AccountId);

        var asset = await _mainDb.Set<Asset>()
            .FirstOrDefaultAsync(a => a.PublicId == request.AssetPublicId);

        if (asset == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid limit order request received");
            return BadRequest(ModelState);
        }

        var orderRequest = new OrderRequest
        {
            ConId = asset.ContractId,
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
        public required string AccountId { get; set; }

        public required Guid AssetPublicId { get; set; }

        [RegularExpression("^(BUY|SELL)$", ErrorMessage = "Side must be BUY or SELL")]
        public required string Side { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public required int Quantity { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public required double Price { get; set; }

        public string Tif { get; set; } = "DAY";
        public string Exchange { get; set; } = "SMART";
    }
}