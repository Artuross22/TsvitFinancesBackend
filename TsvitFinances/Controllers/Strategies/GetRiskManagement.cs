using Data.Modelsl;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace TsvitFinances.Controllers.Strategies;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class GetRiskManagement : Controller
{
    readonly protected MainDb _mainDb;

    public GetRiskManagement(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet("{publicId}")]
    public async Task<IActionResult> Index(Guid publicId)
    {
        var strategy = await _mainDb.Set<Strategy>()
            .Where(s => s.RiskManagement.PublicId == publicId)
            .Select(s => new
            {
                StrategyPublicId = s.PublicId,
                RiskToRewardRatio = s.RiskManagement.RiskToRewardRatio,
                BaseRiskPercentage = s.RiskManagement.BaseRiskPercentage,
            })
            .FirstOrDefaultAsync();

        if (strategy == null)
        {
            return NotFound();
        }

        return Json(strategy);
    }
}
