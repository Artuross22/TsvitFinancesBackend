using Data;
using Data.Models;
using Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Strategies;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class ManagePositionRule : Controller
{
    readonly protected MainDb _mainDb;

    public ManagePositionRule(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet("{publicId}")]
    public async Task<ActionResult> Index(Guid publicId)
    {
        var positionRules = await _mainDb.Set<PositionRule>()
            .Where(a => a.PositionManagement.PublicId == publicId)
            .Select(a => new _PositionRule
            {
                Id = a.Id,
                MinimumCorrectionPercent = a.MinimumCorrectionPercent,
                TimeFrame = a.TimeFrame,
            })
            .ToListAsync();

        if (positionRules == null)
        {
            return NotFound();
        }

        return Ok(new BindingModel
        {
            PublicId = publicId,
            PositionRules = positionRules
        });
    }

    [HttpPost]
    public async Task<ActionResult> Index(BindingModel model)
    {
        var positionManagement = await _mainDb.Set<PositionManagement>()
            .Where(pm => pm.PublicId == model.PublicId)
            .Include(pm => pm.PositionRules)
            .FirstOrDefaultAsync();

        if (positionManagement == null)
        {
            return NotFound();
        }

        var currentPositionRuleIds = positionManagement.PositionRules
            .Select(pm => pm. Id)
            .ToList();

        foreach (var positionRule in model.PositionRules)
        {
            if (currentPositionRuleIds.Contains(positionRule.Id))
            {
                var currentPositionRule = positionManagement.PositionRules
                    .Single(r => r.Id == positionRule.Id);

                currentPositionRule.MinimumCorrectionPercent = positionRule.MinimumCorrectionPercent;
                currentPositionRule.TimeFrame = positionRule.TimeFrame;
            }
            else
            {
                _mainDb.Add(new PositionRule
                {
                    PublicId = Guid.NewGuid(),
                    MinimumCorrectionPercent = positionRule.MinimumCorrectionPercent,
                    TimeFrame = positionRule.TimeFrame,
                    PositionManagementId = positionManagement.Id,
                    PositionManagement = positionManagement,
                });
            }
        }

        var ruleIds = model.PositionRules
            .Select(p => p.Id)
            .ToHashSet();

        var removePositionRules = positionManagement.PositionRules
            .Where(pr => !ruleIds.Contains(pr.Id))
            .ToList();

        if (removePositionRules.Any())
        {
            _mainDb.RemoveRange(removePositionRules);
        }

        await _mainDb.SaveChangesAsync();

        return Ok(positionManagement.PublicId);
    }

    public class BindingModel
    {
        public Guid PublicId { get; set; }

        public List<_PositionRule> PositionRules { get; set; } = [];
    }

    public class _PositionRule
    {
        public int Id { get; init; }

        public required int MinimumCorrectionPercent { get; set; }

        public required TimeFrame TimeFrame { get; set; }
    }
}