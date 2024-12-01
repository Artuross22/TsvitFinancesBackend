using Data.Db;
using Data.Models;
using Data.Modelsl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            model.Position.SellTargets = _salesTargets(strategy.PositionManagement.ScalingOut!.Value, asset.BoughtFor);
        }

        if (strategy.PositionManagement.ScalingIn != null)
        {
            model.Position.BuyTargets = _buyingTargets(strategy.PositionManagement.ScalingIn!.Value, asset.BoughtFor);
        }

        return Ok();
    }

    private List<decimal> _salesTargets(decimal procent, decimal boughtFor)
    {
        List<decimal> targets = new List<decimal>();

        decimal totalPercent = 100m;
        decimal percentApplied = 0m;
        decimal currentSum = boughtFor;

        while (percentApplied < totalPercent)
        {
            currentSum += boughtFor * (procent / 100m);
            percentApplied += procent;
            targets.Add(currentSum);
        }

        return targets;
    }

    private List<decimal> _buyingTargets(decimal procent, decimal boughtFor)
    {
        List<decimal> targets = new List<decimal>();

        decimal totalPercent = 100m;
        decimal percentApplied = 0m;
        decimal currentSum = boughtFor;

        while (percentApplied < totalPercent)
        {
            currentSum += boughtFor * (procent / 100m);
            percentApplied += procent;
            targets.Add(currentSum);
        }

        return targets;
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
