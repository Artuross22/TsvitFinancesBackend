using Data.Modelsl;
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
                PublicId = s.PublicId,
                ScalingOut = s.ScalingOut,
                ScalingIn = s.ScalingIn,
                AverageLevel = s.AverageLevel,
                PositionScalings = s.PositionScalings.Select(p => new 
                {
                   p.PublicId,
                   p.EquityPercentage,
                   p.PositionType
                }).ToList()

            })
            .FirstOrDefaultAsync();

        if (strategy == null)
        {
            return NotFound();
        }

        return Json(strategy);
    }
}
