using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.PositionEntries;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class UpdateCharts : Controller
{
    readonly protected MainDb _mainDb;

    public UpdateCharts(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpPut]
    public async Task<IActionResult> Index(UpdateChartDto model)
    {
        var chart = await _mainDb.Set<Chart>()
            .FirstOrDefaultAsync(c => c.Id == model.Id);

        if (chart is null)
        {
            return NotFound();
        }

        chart.FileName = model.Name;
        chart.Description = model.Description;

        await _mainDb.SaveChangesAsync();

        return Ok();
    }

    public class UpdateChartDto
    {
        public required Guid AssetId { get; set; }

        public required int Id { get; set; }

        public required string Name { get; set; }

        public string? Description { get; set; }
    }
}
