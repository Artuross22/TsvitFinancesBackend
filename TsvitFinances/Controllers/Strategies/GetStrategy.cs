using Data;
using Data.Models;
using Data.Modelsl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Strategies;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class GetStrategy : Controller
{
    readonly protected MainDb _mainDb;
    public GetStrategy(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet("{publicId}/{userId}")]
    public async Task<IActionResult> Index(Guid publicId, string userId)
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

        var financeData = await _mainDb.Set<FinanceData>()
            .FirstOrDefaultAsync(fd => fd.StrategyId == strategy.Id);

        var model = new BindingModel
        {
            PublicId = strategy.PublicId,
            Name = strategy.Name,
            PositionManagement = new _PositionManagement
            {
                Id = strategy.PositionManagement.Id,
                PublicId = strategy.PositionManagement.PublicId,
                AverageLevel = strategy.PositionManagement.AverageLevel,
            },

            RiskManagement = new _RiskManagement
            {
                Id = strategy.RiskManagement.Id,
                PublicId = strategy.RiskManagement.PublicId,
                RiskToRewardRatio = strategy.RiskManagement.RiskToRewardRatio,
                BaseRiskPercentage = strategy.RiskManagement.BaseRiskPercentage,
                Name = strategy.RiskManagement.Name,
            },

            FinanceData = new _FinanceData()
        };

        if (financeData != null)
        {

            model.FinanceData.PublicId = financeData.PublicId;
        }

        return Json(model);
    }

    public class BindingModel
    {
        public required Guid PublicId { get; set; }
        public required string Name { get; set; }
        public _RiskManagement? RiskManagement { get; set; }
        public _PositionManagement? PositionManagement { get; set; }
        public _FinanceData? FinanceData { get; set; }
    }

    public class _RiskManagement
    {
        public required int Id { get; set; }
        public required Guid PublicId { get; set; }
        public required string Name { get; set; }
        public required decimal BaseRiskPercentage { get; set; }
        public required decimal RiskToRewardRatio { get; set; }
        public int HedgeId { get; set; }
        public Hedge? Hedge { get; set; }
    }

    public class _FinanceData
    {
        public Guid PublicId { get; set; }
    }

    public class _PositionManagement
    {
        public required int Id { get; set; }
        public required Guid PublicId { get; set; }
        public required decimal AverageLevel { get; set; }
    }

    public class _PositionScaling
    {
        public required string PublicId { get; set; }
        public required string EquityPercentage { get; set; }
    }

    public class _Diversification
    {
        public required string Name { get; set; }
    }

    public class _Hedge
    {
        public string Name { get; set; }
    }
}
