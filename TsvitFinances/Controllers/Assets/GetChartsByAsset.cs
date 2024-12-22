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
        var charts = await _mainDb.Set<Asset>().Where(a => a.PublicId == id)
            .Include(a => a.Charts)
            .FirstOrDefaultAsync();

        if (charts is null)
        {
            return NotFound();
        }

        var output = new GetCharts
        {
            AssetPublicId = id,
        };

        foreach (var item in charts.Charts)
        {
            output.Charts?.Add(new GetCharts._Chart
            {
                Id = item.Id,
                Name = item.FileName,
                Description = item.Description,
                ChartsPath = item.FilePath.Substring(item.FilePath.IndexOf("public") + "public".Length).Replace("\\", "/"),
            });
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

            public string Description { get; set; } = string.Empty;

            public string ChartsPath { get; set; } = string.Empty;
        }
    }
}
