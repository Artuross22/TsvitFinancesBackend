using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Assets;

[Route("api/[controller]")]
[ApiController]
public class PositionEntryByAsset : Controller
{
    readonly protected MainDb _mainDb;

    public PositionEntryByAsset(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Invoke(Guid id)
    {
        var positionEntryNotes = await _mainDb.Set<PositionEntryNote>()
            .Where(a => a.Asset.PublicId == id)
            .Include(p => p.Charts)
            .ToListAsync();

        if (positionEntryNotes is null)
        {
            return NotFound();
        }

        var output = new BindingModel
        {
            AssetPublicId = id,
            PositionEntries = new List<BindingModel.PositionEntry>()
        };

        foreach (var positionEntryNote in positionEntryNotes)
        {
            var positionEntry = new BindingModel.PositionEntry
            {
                PublicId = positionEntryNote.PublicId,
                Note = positionEntryNote.Note,
                Charts = new List<_Chart>()
            };

            if (positionEntryNote.Charts != null)
            {
                foreach (var chart in positionEntryNote.Charts)
                {
                    positionEntry.Charts.Add(new _Chart
                    {
                        Id = chart.Id,
                        Name = chart.FileName,
                        Description = chart.Description,
                        ChartsPath = chart.FilePath.Substring(chart.FilePath.IndexOf("public") + "public".Length).Replace("\\", "/")
                    });
                }
            }

            output.PositionEntries.Add(positionEntry);
        }

        return Ok(output);
    }

    public class BindingModel
    {
        public required Guid AssetPublicId { get; set; }

        public List<PositionEntry>? PositionEntries { get; set; } = [];

        public class PositionEntry
        {
            public required Guid PublicId { get; set; }

            public string? Note { get; set; } = string.Empty;

            public List<_Chart>? Charts { get; set; }
        }
    }
    public class _Chart
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        public string ChartsPath { get; set; } = string.Empty;
    }
}