using Brokers.IBKR.Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace TsvitFinances.Controllers.InteractiveBrokers;

public class ConfirmOrder : Controller
{
    private readonly IBKRClient _ibkrApiService;
    private readonly ILogger<ConfirmOrder> _logger;

    public ConfirmOrder(IBKRClient ibkrApiService, ILogger<ConfirmOrder> logger)
    {
        _ibkrApiService = ibkrApiService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<OrderConfirmationResponse>> Invoke([FromBody] ConfirmOrderRequest request)
    {
        _logger.LogInformation("Order confirmation request received for reply ID: {ReplyId}, confirmed: {Confirmed}",
            request.ReplyId, request.Confirmed);

        if (string.IsNullOrWhiteSpace(request.ReplyId))
        {
            _logger.LogWarning("Invalid reply ID for order confirmation");
            return BadRequest("Reply ID is required");
        }

        var result = await _ibkrApiService.ConfirmOrderAsync(request.ReplyId, request.Confirmed);

        if (result.Success)
        {
            _logger.LogInformation("Order confirmation successful for reply ID: {ReplyId}", request.ReplyId);
            return Ok(result.Data);
        }

        _logger.LogError("Failed to confirm order for reply ID: {ReplyId}. Error: {Error}", request.ReplyId, result.Error);
        return BadRequest(result.Error);
    }

    public class OrderConfirmationResponse
    {
        public string ReplyId { get; set; }
        public bool Success { get; set; }
        public string Status { get; set; } // "CONFIRMED", "REJECTED", "PENDING"
        public string OrderId { get; set; }
        public string Message { get; set; }
        public DateTime ConfirmationTime { get; set; }
    }

    public class ConfirmOrderRequest
    {
        public string ReplyId { get; set; }
        public bool Confirmed { get; set; } = true;
    }
}
