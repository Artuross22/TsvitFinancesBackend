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

    [HttpGet("{publicId}/{userId}")]
    public async Task<ActionResult> Index(Guid publicId, string userId)
    {
        var asset = await _mainDb.Set<Asset>()
            .Include(a => a.SalesLevels)
            .Include(a => a.PurchaseLevels)
            .Include(a => a.AppUser.BalanceFlows)
            .Where(a => a.AppUser.Id == userId && a.PublicId == publicId)
            .FirstOrDefaultAsync();

        if (asset == null)
        {
            return NotFound();
        }

        var strategy = await _mainDb.Set<Strategy>()
            .Include(s => s.RiskManagement.Diversification)
            .Include(s => s.PositionManagement)
            .FirstOrDefaultAsync(rm => rm.Id == asset.StrategyId);

        if (strategy == null)
        {
            return NotFound();
        }

        var model = new OutputModel();

        await _riskManagement(asset, strategy, model);

        _positionManagement(asset, strategy, model);

        return Ok(model);
    }

    private void _positionManagement(Asset? asset, Strategy strategy, OutputModel model)
    {
        if (strategy.PositionManagement?.ScalingOut != null)
        {
            var percentSellTargets = _findPercentTargets(
                 strategy.PositionManagement.ScalingOut!.Value,
                 asset.BoughtFor,
                 isSaleStrategy: true);

            model.Position.SellTargets = percentSellTargets;

            if (asset.SalesLevels != null)
            {
                var targets = asset.SalesLevels.Select(s => new TargetLevels
                {
                    Level = s.Level,
                    AverageLevel = s.AverageLevel
                })
                .ToList();

                model.Position.SellTargets = _generateTargets(
                strategy.PositionManagement.ScalingOut!.Value,
                strategy.PositionManagement.AverageLevel,
                percentSellTargets,
                targets,
                isSaleStrategy: true);
            }
        }

        if (strategy.PositionManagement?.ScalingIn != null)
        {
            var percentBuyLevels = _findPercentTargets(
                strategy.PositionManagement.ScalingOut!.Value,
                asset.BoughtFor,
                isSaleStrategy: false);

            model.Position.BuyTargets = percentBuyLevels;

            if (asset.PurchaseLevels != null)
            {
                var target = asset.PurchaseLevels.Select(s => new TargetLevels
                {
                    Level = s.Level,
                    AverageLevel = s.AverageLevel
                })
                .ToList();

                model.Position.BuyTargets = _generateTargets(
                    strategy.PositionManagement.ScalingIn!.Value,
                    strategy.PositionManagement.AverageLevel,
                    percentBuyLevels,
                    target,
                    isSaleStrategy: false);
            }
        }
    }

    private async Task _riskManagement(Asset asset, Strategy strategy, OutputModel model)
    {
        if (strategy.RiskManagement != null)
        {
            var baseRiskPercentage = strategy.RiskManagement.BaseRiskPercentage;
            var riskToRewardRatio = strategy.RiskManagement.RiskToRewardRatio;

            if (asset.AppUser.BalanceFlows != null)
            {
                var balance = asset.AppUser.BalanceFlows
                    .Where(a => a.Balance == Balance.Total)
                    .Sum(a => a.Sum);

                model.Risk.BaseRisk = _calculateBaseRisk(balance, baseRiskPercentage);
            }

            model.Risk.RiskToReward = _calculateRiskToReward(asset.BoughtFor, baseRiskPercentage, riskToRewardRatio);

            if (strategy.RiskManagement.Diversification != null)
            {
                model.Risk.Diversifications = await _diversificationCalculation(asset.AppUserId, strategy);
            }
        }
    }

    private async Task<List<Diversification>> _diversificationCalculation(string userId, Strategy strategy)
    {
        var assets = await _mainDb.Set<Asset>()
            .Where(a => a.AppUser.Id == userId)
            .Where(a => a.IsActive)
            .ToListAsync();

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
        decimal riskAmount = boughtFor * (baseRiskPercentage / 100);

        //decimal stopLoss = boughtFor - riskAmount;

        decimal rewardAmount = riskAmount * riskToRewardRatio;

        decimal takeProfit = boughtFor + rewardAmount;

        return takeProfit;
    }

    private List<Target> _generateTargets(
    decimal percent,
    decimal defaultRange,
    List<Target> targets,
    List<TargetLevels> targetLevels,
    bool isSaleStrategy)
    {
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

        var closest = _findClosestLevel(targets, levelTargets, 5m);

        return closest;
    }

    private static List<Target> _findPercentTargets(decimal percent, decimal boughtAssetFor, bool isSaleStrategy)
    {
        List<Target> percentLevelTargets = new List<Target>();

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
            percentLevelTargets.Add(new Target { Percentage = basePrice });
        }

        return percentLevelTargets;
    }

    private List<Target> _findClosestLevel(List<Target> targets, List<decimal> levelTargetIncRanges, decimal range)
    {
        var levelRanges = new List<Target>();

        foreach (var item in targets)
        {
            decimal maximumIndentation = (item.Percentage * range) / 100m;

            var levelRange = new Target
            {
                Percentage = item.Percentage
            };

            var passingLevels = levelTargetIncRanges
                .Where(levelTargetIncRange => Math.Abs(levelTargetIncRange - item.Percentage) <= maximumIndentation)
                .ToList();

            if (passingLevels.Any())
            {
                levelRange.Start = passingLevels.First();
                levelRange.End = passingLevels.Last();
            }

            levelRanges.Add(levelRange);
        }

        return levelRanges;
    }

    public class Diversification
    {
        public decimal TotalNicheSum { get; set; }

        public decimal RecommendedNichePercentage { get; set; }

        public required Sector Sector { get; set; }

        public required decimal Total { get; set; }
    }

    public class Target
    {
        public decimal? Start { get; set; }
        public decimal? End { get; set; }

        public decimal Percentage { get; set; }
    }

    public class TargetLevels
    {
        public required decimal Level { get; set; }
        public required decimal? AverageLevel { get; set; }
    }

    public class OutputModel
    {
        public _Position Position { get; set; } = new _Position();
        public _Risk Risk { get; set; } = new _Risk();

        public class _Position
        {
            public List<Target> BuyTargets { get; set; } = [];

            public List<Target> SellTargets { get; set; } = [];
        }

        public class _Risk
        {
            public decimal BaseRisk { get; set; }

            public decimal RiskToReward { get; set; }

            public List<Diversification> Diversifications { get; set; } = [];
        }
    }
}