using Brokers.IBKR.Client.Models;
using Brokers.IBKR.Client.Services;
using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TsvitFinances.Controllers.InteractiveBrokers;

[Route("api/[controller]")]
public class PlaceMarketOrder : Controller
{
    private readonly IBKRClient _ibkrService;
    private readonly ILogger<PlaceMarketOrder> _logger;
    protected readonly MainDb _mainDb;

    public PlaceMarketOrder(IBKRClient ibkrApiService, ILogger<PlaceMarketOrder> logger, MainDb main)
    {
        _ibkrService = ibkrApiService;
        _logger = logger;
        _mainDb = main;
    }

    [HttpPost]
    public async Task<ActionResult<List<OrderResponse>>> Invoke([FromBody] PlaceMarketOrderRequest request)
    {
        _logger.LogInformation("Market order request received: {Side} {Quantity} for contract {ConId}, account: {AccountId}",
            request.Side, request.Quantity, 12, request.AccountId);

        var asset = await _mainDb.Set<Asset>()
            .FirstOrDefaultAsync(a => a.PublicId == request.AssetPublicId);

        if (asset == null)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid market order request received");
            return BadRequest(ModelState);
        }

        var result = await _ibkrService.PlaceMarketOrderAsync(request.AccountId, asset.ContractId, request.Side, request.Quantity);

        if (result.Success)
        {
            _logger.LogInformation("Market order placed successfully for account: {AccountId}", request.AccountId);

            asset.Buy(request.Quantity, asset.CurrentPrice);
            await _mainDb.SaveChangesAsync();

            return Ok(result.Data);
        }

        _logger.LogError("Failed to place market order for account: {AccountId}. Error: {Error}", request.AccountId, result.Error);
        return BadRequest(result.Error);
    }

    public class PlaceMarketOrderRequest
    {
        public required string AccountId { get; set; }

        public required Guid AssetPublicId { get; set; }

        [RegularExpression("^(BUY|SELL)$", ErrorMessage = "Side must be BUY or SELL")]
        public required string Side { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
        public required int Quantity { get; set; }
    }
}
