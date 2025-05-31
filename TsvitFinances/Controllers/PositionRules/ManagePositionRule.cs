using Data;
using Data.Models;
using Data.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.PositionRules;

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
                PublicId = a.PublicId,
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
        var positionRules = await _mainDb.Set<PositionRule>()
            .Where(a => a.PositionManagement.PublicId == model.PublicId)
            .ToListAsync();

        if (positionRules == null)
        {
            return NotFound();
        }

        var positionRuleIds = positionRules
            .Select(pr => pr.Id)
            .ToList();

        foreach (var positionRule in model.PositionRules)
        {
            if (positionRuleIds.Contains(positionRule.Id))
            {
                var currentPositionRule = positionRules.Single(r => r.Id == positionRule.Id);

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
                    PositionManagementId = positionRules.First().PositionManagementId,
                    PositionManagement = positionRules.First().PositionManagement,
                });
            }
        }

        var ruleIds = model.PositionRules
            .Select(p => p.Id)
            .ToHashSet();

        var removePositionRules = positionRules
            .Where(pr => !ruleIds.Contains(pr.Id))
            .ToList();

        if (removePositionRules.Any())
        {
            _mainDb.RemoveRange(removePositionRules);
        }

        await _mainDb.SaveChangesAsync();

        return Ok(positionRules.First().PositionManagementId);
    }
    public class BindingModel
    {
        public Guid PublicId { get; set; }

        public List<_PositionRule> PositionRules { get; set; } = [];
    }
    public class _PositionRule
    {
        public required int Id { get; init; }

        public required Guid PublicId { get; init; }

        public required int MinimumCorrectionPercent { get; set; }

        public required TimeFrame TimeFrame { get; set; }
    }
}
