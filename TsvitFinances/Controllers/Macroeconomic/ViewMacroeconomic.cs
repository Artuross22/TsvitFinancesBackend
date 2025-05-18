using Data;
using Data.Models;
using Data.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Macroeconomic;

[Route("api/[controller]")]
[ApiController]
public class ViewMacroeconomic : Controller
{
    protected readonly MainDb _mainDb;

    public ViewMacroeconomic(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet("{publicId}")]
    public async Task<IActionResult> Index(Guid publicId)
    {
        var macroeconomicAnalysis = await _mainDb.Set<MacroeconomicAnalysis>()
            .Where(ma => ma.PublicId == publicId)
            .Select(ma => new BindingModel
            {
                PublicId = ma.PublicId,
                Title = ma.Title,
                EconomicType = ma.EconomicType,
                Description = ma.Description,
                MacroeconomicEvents = (ma.MacroeconomicEvents ?? new List<MacroeconomicEvent>())
                    .Select(me => new _MacroeconomicEvent
                    {
                        PublicId = me.PublicId,
                        CreateAt = me.CreateAt,
                        Description = me.Description,
                        Rating = me.Rating,
                        Source = me.Source,
                        Title = me.Title,
                    }).ToList()
            })
            .SingleOrDefaultAsync();

        if (macroeconomicAnalysis == null)
        {
            return NotFound(macroeconomicAnalysis);
        }

        return Ok(macroeconomicAnalysis);
    }

    public class BindingModel
    {
        public required Guid PublicId { get; set; }

        public required string Title { get; set; }

        public required string Description { get; set; }

        public required EconomicType EconomicType { get; set; }

        public IReadOnlyList<_MacroeconomicEvent>? MacroeconomicEvents { get; set; }
    }

    public class _MacroeconomicEvent
    {
        public required Guid PublicId { get; set; }

        public required string Title { get; set; }

        public required string Description { get; set; }

        public required int Rating { get; set; }

        public required DateTime CreateAt { get; set; }

        public required string Source { get; set; }
    }
}