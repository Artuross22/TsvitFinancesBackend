using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Data.Models;

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
        var strategy = await _mainDb.Set<PositionManagement>()
            .Where(s => s.PublicId == publicId)
            .Select(s => new
            {
                StrategyPublicId = s.PublicId,
                s.PublicId,
                s.ScalingOut,
                ScalingOutPositionDistribution = 100 / s.ScalingOut,
                s.ScalingIn,
                ScalingInPositionDistribution = 100 / s.ScalingIn,
                s.AverageLevel,
                PositionScalings = s.PositionScalings.Select(p => new 
                {
                   p.PublicId,
                   p.EquityPercentage,
                   p.PositionType
                })
                .ToList()
            })
            .FirstOrDefaultAsync();

        if (strategy == null)
        {
            return NotFound();
        }

        return Json(strategy);
    }
}
