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

    public class LiveOrder
    {
        public string OrderId { get; set; }
        public string AccountId { get; set; }
        public string Symbol { get; set; }
        public string Side { get; set; } // BUY, SELL
        public decimal Quantity { get; set; }
        public decimal FilledQuantity { get; set; }
        public string OrderType { get; set; } // MKT, LMT, STP
        public decimal? Price { get; set; }
        public string Status { get; set; } // Submitted, Filled, Cancelled
        public DateTime SubmittedTime { get; set; }
    }
}