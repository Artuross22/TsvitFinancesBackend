using Data;
using Data.Models;
using Data.Models.Enums;
using Data.Modelsl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Strategies;

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
    public async Task<IActionResult> Invoke(Guid publicId, string userId)
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

        var model = new BindingModel
        {
            PublicId = strategy.PublicId,
            Name = strategy.Name,
            Description = strategy.Description,
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

        model.FinanceData.PublicId = await _mainDb.Set<FinanceData>()
            .Where(fd => fd.StrategyId == strategy.Id)
            .Select(fd => fd.PublicId)
            .FirstOrDefaultAsync();

        model.MacroeconomicEvents = _mainDb.Set<StrategyMacroeconomicEvent>()
            .Where(sme => sme.StrategyId == strategy.Id)
            .Select(sme => sme.MacroeconomicEvent)
            .Select(me => new _MacroeconomicEvents
            {
                PublicId = me!.PublicId,
                CreateAt = me.CreateAt,
                Title = me.Title,
                EconomicType = me.MacroeconomicAnalyses.EconomicType
            })
            .OrderBy(me => me.CreateAt)
            .ToList();

        return Json(model);
    }

    public class BindingModel
    {
        public required Guid PublicId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public _RiskManagement? RiskManagement { get; set; }
        public _PositionManagement? PositionManagement { get; set; }
        public _FinanceData? FinanceData { get; set; }

        public List<_MacroeconomicEvents>? MacroeconomicEvents { get; set; }
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

    public class _MacroeconomicEvents
    {
        public required Guid PublicId { get; set; }
        public required string Title { get; set; }
        public DateTime CreateAt { get; set; }
        public EconomicType EconomicType { get; set; }
    }
}
