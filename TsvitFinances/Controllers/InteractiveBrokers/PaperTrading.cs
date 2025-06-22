using Brokers.IBKR.Client.Models;
using Brokers.IBKR.Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace TsvitFinances.Controllers.InteractiveBrokers;

[ApiController]
[Route("api/[controller]")]
public class PaperTrading : ControllerBase
{
    private readonly PaperTradingDemoService _demoService;
    private readonly IBKRPaperTradingService _ibkrService;

    public PaperTrading(PaperTradingDemoService demoService, IBKRPaperTradingService ibkrService)
    {
        _demoService = demoService;
        _ibkrService = ibkrService;
    }

    [HttpGet("accounts")]
    public async Task<IActionResult> GetPaperAccounts()
    {
        var result = await _ibkrService.GetPaperAccountsAsync();
        return result.Success ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpGet("balance/{accountId}")]
    public async Task<IActionResult> GetBalance(string accountId)
    {
        var result = await _ibkrService.GetPaperBalanceAsync(accountId);
        return result.Success ? Ok(result.Data) : BadRequest(result.Error);
    }

    [HttpPost("demo-trade")]
    public async Task<IActionResult> RunDemoTrade()
    {
        var success = await _demoService.DemoTradingScenarioAsync();
        return success ? Ok("Demo trading completed successfully") : BadRequest("Demo trading failed");
    }

    [HttpPost("paper-order")]
    public async Task<IActionResult> PlacePaperOrder([FromBody] PaperOrderRequest request)
    {
        var orderRequest = new OrderRequest
        {
            ConId = request.ConId,
            OrderType = request.OrderType,
            Side = request.Side,
            Quantity = request.Quantity,
            Price = request.Price,
            Tif = request.Tif
        };

        var result = await _ibkrService.PlacePaperOrderAsync(request.AccountId, orderRequest);
        return result.Success ? Ok(result.Data) : BadRequest(result.Error);
    }
}
