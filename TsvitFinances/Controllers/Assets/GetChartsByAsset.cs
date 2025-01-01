using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Assets;


[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class GetChartsByAsset : Controller
{
    readonly protected MainDb _mainDb;

    public GetChartsByAsset(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Index(Guid id)
    {
        var positionEntryNotes = await _mainDb.Set<PositionEntryNote>()
            .Where(a => a.Asset.PublicId == id)
            .Include(p => p.Charts)   
            .ToListAsync();

        if (positionEntryNotes is null)
        {
            return NotFound();
        }

        var output = new GetCharts
        {
            AssetPublicId = id,
        };

        foreach (var positionEntryNote in positionEntryNotes)
        {
            if(positionEntryNote.Charts is null)
            {
                continue;
            }

            foreach (var chart in positionEntryNote.Charts)
            {
                output.Charts?.Add(new GetCharts._Chart
                {
                    Id = chart.Id,
                    Name = chart.FileName,
                    Description = chart.Description,
                    ChartsPath = chart.FilePath.Substring(chart.FilePath.IndexOf("public") + "public".Length).Replace("\\", "/"),
                });
            }
        }

        return Ok(output);
    }
    public class GetCharts
    {
        public required Guid AssetPublicId { get; set; }
        public IList<_Chart>? Charts { get; set; } = [];

        public class _Chart
        {
            public int Id { get; set; }

            public string Name { get; set; } = string.Empty;

            public string? Description { get; set; } = string.Empty;

            public string ChartsPath { get; set; } = string.Empty;
        }
    }
}
