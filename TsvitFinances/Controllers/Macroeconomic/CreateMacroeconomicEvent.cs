using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Macroeconomic;

[Route("api/[controller]")]
[ApiController]
public class CreateMacroeconomicEvent : Controller
{
    protected readonly MainDb _mainDb;

    public CreateMacroeconomicEvent(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpPost]
    public async Task<IActionResult> Invoke(BindingModel model)
    {
        var macroeconomicAnalysis = await _mainDb.Set<MacroeconomicAnalysis>()
            .FirstOrDefaultAsync(u => u.PublicId == model.MacroeconomicAnalysisId);

        if (macroeconomicAnalysis == null)
        {
            return NotFound();
        }

        _mainDb.Add(new MacroeconomicEvent
        {
            PublicId = Guid.NewGuid(),
            Description = model.Description,
            Title = model.Title,
            Rating = model.Rating,
            Source = model.Source,
            CreateAt = DateTime.UtcNow,
            MacroeconomicAnalysisId = macroeconomicAnalysis.Id,
            MacroeconomicAnalyses = macroeconomicAnalysis,
        });

        await _mainDb.SaveChangesAsync();

        return Ok();
    }

    public class BindingModel
    {
        public required Guid MacroeconomicAnalysisId { get; set; }

        public required string Title { get; set; }

        public required string Description { get; set; }

        public required string Source { get; set; }

        public int Rating { get; set; }
    }
}