using Data.Db;
using Data.Models;
using Data.Models.Enums;
using Data.Modelsl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsvitFinances.Dto.Strategy;
using TsvitFinances.Dto.Strategy.PositionEntry;
using TsvitFinances.Dto.Strategy.RiskManagement;

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
            .Include(s => s.RiskManagement)
            .Include(s => s.PositionManagement)
            .Where(s => s.PublicId == publicId)
            .Where(s => s.AppUser.Id == userId)
            .FirstOrDefaultAsync();

        if (strategy == null)
        {
            return NotFound();
        }

        return Json(strategy);
    }

    [HttpGet("GetRiskManagement/{publicId}")]
    public async Task<IActionResult> GetRiskManagement(Guid publicId)
    {
        var strategy = await _mainDb.Set<Strategy>()
            .Where(s => s.RiskManagement.PublicId == publicId)
            .Select(s => new
            {
                StrategyPublicId = s.PublicId,
                RiskToRewardRatio = s.RiskManagement.RiskToRewardRatio,
                BaseRiskPercentage = s.RiskManagement.BaseRiskPercentage,
                Category = s.RiskManagement.Category
            })
            .FirstOrDefaultAsync();

        if (strategy == null)
        {
            return NotFound();
        }

        return Json(strategy);
    }

    [HttpPut("PutRiskManagement")]
    public async Task<IActionResult> PutRiskManagement(RiskManagementDto model)
    {
        var riskManagement = await _mainDb.Set<RiskManagement>()
            .Where(s => s.PublicId == model.PublicId)
            .FirstOrDefaultAsync();

        if (riskManagement == null)
        {
            return NotFound();
        }

        riskManagement.Category = (RiskCategory)model.Category;
        riskManagement.RiskToRewardRatio = model.RiskToRewardRatio;
        riskManagement.BaseRiskPercentage = model.BaseRiskPercentage;

        _mainDb.SaveChanges();

        return Ok();
    }

    [HttpGet("GetPositionManagement/{publicId}")]
    public async Task<IActionResult> GetPositionManagement(Guid publicId)
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

    [HttpPut("PutPositionManagement")]
    public async Task<IActionResult> PutPositionManagement(PositionManagementDto model)
    {
        var positionManagement = await _mainDb.Set<PositionManagement>()
            .Where(s => s.PublicId == model.PublicId)
            .FirstOrDefaultAsync();

        if (positionManagement == null)
        {
            return NotFound();
        }

        positionManagement.ScalingOut = model.ScalingOut;
        positionManagement.ScalingIn = model.ScalingIn;
        positionManagement.AverageLevel = model.AverageLevel;

        _mainDb.SaveChanges();

        return Ok();
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetStrategies(string id)
    {
        var strategies = await _mainDb.Set<Strategy>()
            .Where(s => s.AppUser.Id == id)
            .Select(s => new ListStrategies
            {
                PublicId = s.PublicId,
                Name = s.Name,
            })
            .ToListAsync();

        if (strategies == null)
        {
            return NotFound();
        }

        return Json(strategies);
    }

    [HttpPost("AddStrategyToAsset")]
    public async Task<IActionResult> AddStrategyToAsset(AddStrategyToAsset model)
    {
        var strategy = await _mainDb.Set<Strategy>()
            .FirstOrDefaultAsync(s => s.PublicId == model.StrategyPublicId);

        var asset = await _mainDb.Set<Asset>()
            .FirstOrDefaultAsync(s => s.PublicId == model.AssetPublicId);

        if (strategy == null || asset == null)
        {
            return NotFound();
        }

       asset.Strategy = strategy;
       await _mainDb.SaveChangesAsync();

        return Ok();
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

        var positionManagement = new Data.Models.PositionManagement
        {
            PublicId = Guid.NewGuid(),
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
