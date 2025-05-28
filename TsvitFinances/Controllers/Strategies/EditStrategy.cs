using Data;
using Data.Models;
using Data.Models.Enums;
using Data.Modelsl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TsvitFinances.Controllers.Strategies;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class EditStrategy : Controller
{
    readonly protected MainDb _mainDb;
    public EditStrategy(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet("{publicId}/{userId}")]
    public async Task<IActionResult> Index(Guid publicId, string userId)
    {
        var strategy = await _mainDb.Set<Strategy>()
            .Include(s => s.StrategyMacroeconomicEvents)
            .FirstOrDefaultAsync(s => s.PublicId == publicId);

        if (strategy is null)
        {
            return NotFound();
        }

        var selectedEventIds = strategy.StrategyMacroeconomicEvents?
            .Select(sme => sme.MacroeconomicEventId)
            .ToHashSet();

        var macroeconomicEvents = await _mainDb.Set<MacroeconomicAnalysis>()
            .Where(ma => ma.AppUser.Id == userId && ma.MacroeconomicEvents != null)
            .SelectMany(ma => ma.MacroeconomicEvents!
                .Select(me => new _MacroeconomicEvents
                {
                    Id = me.Id,
                    PublicId = me.PublicId,
                    EconomicType = ma.EconomicType,
                    Title = me.Title,
                    CreateAt = me.CreateAt,
                    IsSelected = selectedEventIds != null && selectedEventIds.Contains(me.Id)
                }))
            .OrderBy(ma => ma.CreateAt)
            .ToListAsync();


        return Ok(new BindingModelPost
        {
            PublicId = strategy.PublicId,
            Name = strategy.Name,
            Description = strategy.Description,
            MacroeconomicEvents = macroeconomicEvents,
        });
    }

    [HttpPut]
    public async Task<IActionResult> Index(BindingModelPost model)
    {
        var strategy = await _mainDb.Set<Strategy>()
            .Include(s => s.StrategyMacroeconomicEvents!)
            .SingleAsync(s => s.PublicId == model.PublicId);

        strategy.Name = model.Name;
        strategy.Description = model.Description;

        var selectedMacroeconomicEventIds = model.MacroeconomicEvents
            .Where(e => e.IsSelected)
            .Select(e => e.Id)
            .ToHashSet();

        var existingLinks = strategy.StrategyMacroeconomicEvents
            .ToList();

        var existingEventIds = existingLinks
            .Select(me => me.MacroeconomicEventId)
            .ToHashSet();

        var toRemove = existingLinks
            .Where(link => !selectedMacroeconomicEventIds.Contains(link.MacroeconomicEventId))
            .ToList();

        if (toRemove.Any())
        {
            _mainDb.RemoveRange(toRemove);
        }

        var toAddIds = selectedMacroeconomicEventIds
            .Except(existingEventIds)
            .ToList();

        foreach (var eventId in toAddIds)
        {
            _mainDb.Add(new StrategyMacroeconomicEvent
            {
                StrategyId = strategy.Id,
                MacroeconomicEventId = eventId
            });
        }

        await _mainDb.SaveChangesAsync();

        return Ok();
    }

    public class BindingModelPost
    {
        public required Guid PublicId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public List<_MacroeconomicEvents> MacroeconomicEvents { get; set; } = [];
    }

    public class _MacroeconomicEvents
    {
        public required int Id { get; set; }
        public required Guid PublicId { get; set; }
        public required string Title { get; set; }
        public required EconomicType EconomicType { get; set; }
        public bool IsSelected { get; set; }
        public DateTime CreateAt { get; internal set; }
    }
}