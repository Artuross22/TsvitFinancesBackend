using Brokers.IBKR.Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TsvitFinances.Controllers.InteractiveBrokers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class IbkrAuth : Controller
{
    private readonly IBKRClient _ibkrService;

    public IbkrAuth(IBKRClient ibkrService)
    {
        _ibkrService = ibkrService;
    }

    [HttpGet("auth-status")]
    public async Task<IActionResult> GetAuthStatus()
    {
        var result = await _ibkrService.GetAuthStatusAsync();

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }
}