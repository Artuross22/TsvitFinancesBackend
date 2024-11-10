using Data.Db;
using Data.Models;
using Data.Models.Enums;
using Data.Modelsl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsvitFinances.Dto.Strategy;

namespace TsvitFinances.Controllers;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class StrategiesController : Controller
{
    readonly protected MainDb _mainDb;
    public StrategiesController(MainDb mainDb)
    {
       _mainDb = mainDb;
    }

    [HttpGet("{publicId}/{userId}")]
    public async Task<IActionResult> GetStrategy(Guid publicId, string userId)
    {
        var strategy = await _mainDb.Set<Strategy>()
            .Where(s => s.PublicId == publicId)
            .Where(s => s.AppUser.Id == userId)
            .FirstOrDefaultAsync();

        if (strategy == null)
        {
            return NotFound();
        }

        return Json(strategy);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStrategies(string id)
    {
        var strategies = await _mainDb.Set<Strategy>()
            .Where(s => s.AppUser.Id == id)
            .Select(s => new ListStrategies
            {
              PubliceId = s.PublicId,
              Name = s.Name,
            })
            .ToListAsync();

        if (strategies == null)
        {
            return NotFound();
        }

        return Json(strategies);
    }

    [HttpPost]
    public async Task<IActionResult> AddStrategy(AddStrategy model)
    {
        var user = await _mainDb.Set<AppUser>()
            .FirstOrDefaultAsync(c => c.Id == model.UserPublicId);

        if (user == null)
        {
            return NotFound();
        }

        var riskManagement = new RiskManagement
        {
            Name = string.Empty,
            Category = RiskCategory.Low,
            BaseRiskPercentage = 2,
            RiskToRewardRatio = 3,
            HedgeId = null!,
            Hedge = null!,
            DiversificationId = null!,
            Diversification = null!,
        };

        _mainDb.Add(riskManagement);

        var positionManagement = new PositionManagement
        {
            ScalingIn = 0,
            ScalingOut = 20,
            AverageLevel = 5,
        };

        _mainDb.Add(positionManagement);

        var strategy = new Strategy
        {
            PublicId = Guid.NewGuid(),
            AppUser = user,
            AppUserId = user.Id,
            Name = model.Name,
            PositionManagementId = null!,
            PositionManagement = positionManagement,
            RiskManagementId = null!,
            RiskManagement = riskManagement,
        };

        _mainDb.Add(strategy);

        await _mainDb.SaveChangesAsync();

        return Ok();
    }
}
