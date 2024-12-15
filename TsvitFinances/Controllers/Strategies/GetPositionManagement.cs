using Data.Modelsl;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace TsvitFinances.Controllers.Strategies;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class GetPositionManagement : Controller
{
    readonly protected MainDb _mainDb;
    public GetPositionManagement(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet("{publicId}")]
    public async Task<IActionResult> Index(Guid publicId)
    {
        var strategy = await _mainDb.Set<Strategy>()
            .Where(s => s.PositionManagement.PublicId == publicId)
            .Select(s => new
            {
                StrategyPublicId = s.PublicId,
                PublicId = s.PositionManagement.PublicId,
                ScalingOut = s.PositionManagement.ScalingOut,
                ScalingIn = s.PositionManagement.ScalingIn,
                AverageLevel = s.PositionManagement.AverageLevel
            })
            .FirstOrDefaultAsync();

        if (strategy == null)
        {
            return NotFound();
        }

        return Json(strategy);
    }
}
