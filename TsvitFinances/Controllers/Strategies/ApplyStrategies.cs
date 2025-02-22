using Data;
using Data.Models;
using Data.Models.Enums;
using Data.Modelsl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsvitFinances.FinancialHelper;
using TsvitFinances.FinancialHelper.Models;

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
            .Include(s => s.PositionManagement.PositionScalings)
            .FirstOrDefaultAsync(rm => rm.Id == asset.StrategyId);

        if (strategy == null)
        {
            return NotFound();
        }

        var model = new OutputModel();

        _riskManagement(asset, strategy, model);

        if (strategy.PositionManagement.PositionScalings != null)
        {
            _positionManagement(asset, strategy, model);
        }

        return Ok(model);
    }

    private void _positionManagement(Asset? asset, Strategy strategy, OutputModel model)
    {
        var positionScalings = strategy.PositionManagement.PositionScalings
            .Where(p => p.PositionType == PositionType.Short)
            .ToList();

        if (positionScalings.Any())
        {
            var percentSellTargets = _findPercentTargets(
                positionScalings,
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

                model.Position.SellTargets = _findBestTargets(
                positionScalings.Count(),
                strategy.PositionManagement.AverageLevel,
                percentSellTargets,
                targets,
                isSaleStrategy: true);
            }
        }

        var positionScalingIns = strategy.PositionManagement!.PositionScalings
             .Where(p => p.PositionType == PositionType.Long)
             .ToList();

        if (positionScalingIns.Any())
        {
            var percentBuyLevels = _findPercentTargets(
                positionScalingIns,
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

                model.Position.BuyTargets = _findBestTargets(
                    positionScalingIns.Count,
                    strategy.PositionManagement.AverageLevel,
                    percentBuyLevels,
                    target,
                    isSaleStrategy: false);
            }
        }
    }

    private void _riskManagement(Asset asset, Strategy strategy, OutputModel model)
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
                var diversification = strategy.RiskManagement.Diversification.Where(d => d.Sector == asset.Sector).ToList();

                model.Risk.Diversifications = CalculateDiversification.Result(diversification, asset.Sector, asset);
            }
        }
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

    private List<Target> _findBestTargets(
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

        return _findClosestLevel(targets, levelTargets, 5m);
    }

    private static List<Target> _findPercentTargets(
        List<PositionScaling> positionScalings,
        decimal boughtAssetFor,
        bool isSaleStrategy)
    {
        List<Target> percentLevelTargets = new List<Target>();

        var percent = 100 / positionScalings.Count();

        if (percent == default)
        {
            return percentLevelTargets;
        }

        decimal totalPercent = 100m;
        decimal percentApplied = 0m;
        decimal percentageLevel = boughtAssetFor;

        int index = 0;

        while (percentApplied < totalPercent)
        {
            decimal positionScaling = index < positionScalings.Count
                     ? positionScalings[index].EquityPercentage
                     : 0;

            if (isSaleStrategy)
            {
                percentageLevel += boughtAssetFor * (percent / 100m);
            }
            else
            {
                percentageLevel -= boughtAssetFor * (percent / 100m);
            }

            percentApplied += percent;
            percentLevelTargets.Add(new Target
            {
                PercentageLevel = percentageLevel,
                PositionScaling = positionScaling
            });

            index++;
        }

        return percentLevelTargets;
    }

    private List<Target> _findClosestLevel(List<Target> targets, List<decimal> levelTargetIncRanges, decimal range)
    {
        foreach (var target in targets)
        {
            decimal maximumIndentation = (target.PercentageLevel * range) / 100m;

            var passingLevels = levelTargetIncRanges
                .Where(levelTargetIncRange => Math.Abs(levelTargetIncRange - target.PercentageLevel) <= maximumIndentation)
                .ToList();

            if (passingLevels.Any())
            {
                target.Start = passingLevels.First();
                target.End = passingLevels.Last();
            }
        }

        return targets;
    }

    public class Target
    {
        public decimal? Start { get; set; }

        public decimal? End { get; set; }

        public decimal PercentageLevel { get; set; }

        public decimal PositionScaling { get; set; }
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

            public List<DiversificationResult> Diversifications { get; set; } = [];
        }
    }
}