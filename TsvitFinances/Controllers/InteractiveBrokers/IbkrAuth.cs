using Brokers.IBKR.Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace TsvitFinances.Controllers.InteractiveBrokers;

[Route("api/[controller]/[action]")]
public class IbkrAuth : Controller
{
    private readonly IBKRClient _ibkrService;

    public IbkrAuth(IBKRClient ibkrService)
    {
        _ibkrService = ibkrService;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetAuthStatus()
    {
        return Ok(await _ibkrService.GetAuthStatusAsync());
    }
}