using Data.Db;
using Data.Models;
using Data.Modelsl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static TsvitFinances.Controllers.test.ApplyStrategies;

namespace TsvitFinances.Controllers.test;

public class ApplyStrategies : Controller
{
    readonly protected MainDb _mainDb;
    public ApplyStrategies(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet]
    [Route("api/[controller]")]
    public async Task<ActionResult> Index(Guid publicId)
    {
        var asset = await _mainDb.Set<Asset>()
             .FirstOrDefaultAsync(s => s.PublicId == publicId);

        if (asset == null)
        {
            return NotFound();
        }

        var strategy = await _mainDb.Set<Strategy>()
            .Include(s => s.RiskManagement)
            .Include(s => s.PositionManagement)
            .FirstOrDefaultAsync(rm => rm.Id == asset.StrategyId);

        if (strategy == null)
        {
            return NotFound();
        }

        var model = new BindingModel();

        if (strategy.PositionManagement.ScalingOut != null)
        {
            var target = strategy.PositionManagement.SalesLevels.Select(s => new TargetLevels
            {
                Level = s.Level,
                AverageLevel = s.AverageLevel
            })
            .ToList();

            model.Position.SellTargets = _salesTargets(strategy.PositionManagement.ScalingOut!.Value, strategy.PositionManagement.AverageLevel, asset.BoughtFor, target);
        }

        if (strategy.PositionManagement.ScalingIn != null)
        {

            var target = strategy.PositionManagement.PurchaseLevels.Select(s => new TargetLevels
            {
                Level = s.Level,
                AverageLevel = s.AverageLevel
            })
            .ToList();

            model.Position.BuyTargets = _buyingTargets(strategy.PositionManagement.ScalingIn!.Value, asset.BoughtFor, target);
        }

        return Ok();
    }

    private List<decimal> _salesTargets(decimal procent, decimal boughtFor, decimal defaultAverageLevel, List<TargetLevels> targetLevels)
    {
        List<decimal> procentTargets = new List<decimal>();

        decimal totalPercent = 100m;
        decimal percentApplied = 0m;
        decimal percentageLevel = boughtFor;

        while (percentApplied < totalPercent)
        {
            percentageLevel += boughtFor * (procent / 100m);
            percentApplied += procent;
            procentTargets.Add(percentageLevel);
        }

        List<decimal> levelTargets = new List<decimal>();

        foreach (var targetLevel in targetLevels)
        {
            var average = targetLevel.AverageLevel ?? defaultAverageLevel;
            var saleLevel =+ targetLevel.Level * (average / 100m);
            levelTargets.Add(saleLevel);
        }

        List<decimal> bestTargets = new List<decimal>();

        foreach (var num in procentTargets)
        {
            var closest = _findClosestLevel(num, levelTargets, 0.1m);

            if (closest.HasValue)
            {
                bestTargets.Add(closest.Value);
            }
        }

        return bestTargets;
    }

    static decimal? _findClosestLevel(decimal procentTarget, List<decimal> levelTargets, decimal thresholdProcent)
    {
        decimal threshold = procentTarget * thresholdProcent;

        decimal? closest = null;

        foreach (var levelTarget in levelTargets)
        {
            if (Math.Abs(levelTarget - procentTarget) <= threshold)
            {
                if (closest == null || Math.Abs(levelTarget - procentTarget) < Math.Abs(closest.Value - procentTarget))
                {
                    closest = levelTarget;
                }
            }
        }

        return closest;
    }

    private List<decimal> _buyingTargets(decimal procent, decimal boughtFor, List<TargetLevels> targetLevels)
    {
        List<decimal> targets = new List<decimal>();

        decimal totalPercent = 100m;
        decimal percentApplied = 0m;
        decimal currentSum = boughtFor;

        while (percentApplied < totalPercent)
        {
            currentSum -= boughtFor * (procent / 100m);
            percentApplied += procent;
            targets.Add(currentSum);
        }

        return targets;
    }

    public class TargetLevels
    {
        public required decimal Level { get; set; }
        public required decimal? AverageLevel { get; set; }
    }

    public class BindingModel
    {
        public _Position Position { get; set; } = null!;
        public _Risk Risk { get; set; } = null!;

        public class _Position
        {
            public List<decimal> BuyTargets { get; set; } = [];

            public List<decimal> SellTargets { get; set; } = [];
        }

        public class _Risk
        {
            public required string Id { get; set; }
        }
    }
}
