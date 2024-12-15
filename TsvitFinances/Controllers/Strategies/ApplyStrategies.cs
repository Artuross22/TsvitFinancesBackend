using Data;
using Data.Models;
using Data.Models.Enums;
using Data.Modelsl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.test;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class ApplyStrategies : Controller
{
    readonly protected MainDb _mainDb;
    public ApplyStrategies(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet]
    public async Task<ActionResult> Index(Guid publicId, string userId)
    {
        var assets = await _mainDb.Set<Asset>()
             .Include(a => a.AppUser)
             .Where(a => a.AppUser.Id == userId)
             .ToListAsync();

        var asset = assets.FirstOrDefault(a => a.PublicId == publicId);

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

        if (strategy.RiskManagement != null)
        {
            var balance = asset.AppUser.BalanceFlows
                .Where(a => a.Balance == Balance.Total)
                .Sum(a => a.Sum);

            var baseRiskPercentage = strategy.RiskManagement.BaseRiskPercentage;
            var riskToRewardRatio = strategy.RiskManagement.RiskToRewardRatio;

            model.Risk.BaseRisk = _calculateBaseRisk(balance, baseRiskPercentage);
            model.Risk.RiskToReward = _calculateRiskToReward(asset.BoughtFor, baseRiskPercentage, riskToRewardRatio);

            if (strategy.RiskManagement.Diversification != null)
            {
                model.Risk.Diversifications = _diversificationCalculation(assets, strategy);
            }
        }

        if (strategy.PositionManagement.ScalingOut != null)
        {
            var target = strategy.PositionManagement.SalesLevels.Select(s => new TargetLevels
            {
                Level = s.Level,
                AverageLevel = s.AverageLevel
            })
            .ToList();

            model.Position.SellTargets = _generateTargets(
                strategy.PositionManagement.ScalingOut!.Value,
                strategy.PositionManagement.AverageLevel,
                asset.BoughtFor,
                target,
                isSaleStrategy: true);
        }

        if (strategy.PositionManagement.ScalingIn != null)
        {
            var target = strategy.PositionManagement.PurchaseLevels.Select(s => new TargetLevels
            {
                Level = s.Level,
                AverageLevel = s.AverageLevel
            })
            .ToList();

            model.Position.BuyTargets = _generateTargets(
                strategy.PositionManagement.ScalingIn!.Value,
                asset.BoughtFor,
                strategy.PositionManagement.AverageLevel,
                target,
                isSaleStrategy: false);
        }

        return Ok(model);
    }

    private List<Diversification> _diversificationCalculation(List<Asset> assets, Strategy strategy)
    {
        var diversifications = new List<Diversification>();

        var total = assets.Sum(a => a.CurrentPrice);

        foreach (var diversification in strategy.RiskManagement.Diversification)
        {
            var totalNicheSum = assets.Where(a => a.Sector == diversification.Sector).Sum(a => a.CurrentPrice);

            diversifications.Add(new Diversification
            {
                TotalNicheSum = totalNicheSum,
                Total = (totalNicheSum / total) * 100,
                RecommendedNichePercentage = diversification.NichePercentage,
                Sector = diversification.Sector,
            });
        }

        return diversifications;
    }

    private decimal _calculateBaseRisk(decimal balance, decimal baseRiskPercentage)
    {
        return balance * (baseRiskPercentage / 100m);
    }

    private decimal _calculateRiskToReward(decimal boughtFor, decimal baseRiskPercentage, decimal riskToRewardRatio)
    {
        return boughtFor + riskToRewardRatio * baseRiskPercentage;
    }

    private List<Range> _generateTargets(
    decimal percent,
    decimal boughtAssetFor,
    decimal defaultRange,
    List<TargetLevels> targetLevels,
    bool isSaleStrategy)
    {
        List<decimal> percentLevelTargets = new List<decimal>();

        decimal totalPercent = 100m;
        decimal percentApplied = 0m;
        decimal basePrice = boughtAssetFor;

        while (percentApplied < totalPercent)
        {
            if (isSaleStrategy)
            {
                basePrice += boughtAssetFor * (percent / 100m);
            }
            else
            {
                basePrice -= boughtAssetFor * (percent / 100m);
            }

            percentApplied += percent;
            percentLevelTargets.Add(basePrice);
        }

        List<decimal> levelTargets = new List<decimal>();

        foreach (var targetLevel in targetLevels)
        {
            var range = targetLevel.AverageLevel ?? defaultRange;
            decimal calculatedLevel;

            if (isSaleStrategy)
            {
                calculatedLevel = targetLevel.Level - targetLevel.Level * range / 100m;
            }
            else
            {
                calculatedLevel = targetLevel.Level + targetLevel.Level * range / 100m;
            }

            levelTargets.Add(calculatedLevel);
        }

        var bestTargets = new List<Range>();

        foreach (var percentLevelTarget in percentLevelTargets)
        {
            var closest = _findClosestLevel(percentLevelTarget, levelTargets, 5m);
            if (closest != null)
            {
                bestTargets.AddRange(closest);
            }
        }

        return bestTargets;
    }

    private Range? _findClosestLevel(decimal percentLevelTarget, List<decimal> levelTargetIncRanges, decimal range)
    {
        decimal maximumIndentation = (percentLevelTarget * range) / 100m;

        var levelRanges = new List<decimal>();

        foreach (var levelTargetIncRange in levelTargetIncRanges)
        {
            if (Math.Abs(levelTargetIncRange - percentLevelTarget) <= maximumIndentation)
            {
                levelRanges.Add(levelTargetIncRange);
            }
        }

        if (levelRanges.Any())
        {
            return new Range
            {
                Start = levelRanges.Min(),
                End = levelRanges.Max()
            };
        }

        return null;
    }

    public class Diversification
    {
        public decimal TotalNicheSum { get; set; }

        public decimal RecommendedNichePercentage { get; set; }

        public required Sector Sector { get; set; }

        public required decimal Total { get; set; }
}

    public class Range
    {
        public decimal? Start { get; set; }
        public decimal End { get; set; }
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
            public List<Range> BuyTargets { get; set; } = [];

            public List<Range> SellTargets { get; set; } = [];
        }

        public class _Risk
        {
            public decimal BaseRisk { get; set; }

            public decimal RiskToReward { get; set; }

            public List<Diversification> Diversifications { get; set; } = [];
        }
    }
}