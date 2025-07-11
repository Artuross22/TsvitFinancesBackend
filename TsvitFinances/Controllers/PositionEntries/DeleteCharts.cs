using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.PositionEntries;

[Route("api/[controller]")]
[ApiController]
public class DeleteCharts : Controller
{
    readonly protected MainDb _mainDb;

    public DeleteCharts(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpDelete("{id}/{assetId}")]
    public async Task<IActionResult> Invoke(int id, Guid assetId)
    {
        var chart = await _mainDb.Set<Chart>()
            .Where(c => c.PositionEntryNote.Asset.PublicId == assetId && c.Id == id)
            .FirstOrDefaultAsync();

        if (chart is null)
        {
            return NotFound();
        }

        _mainDb.Remove(chart);
        await _mainDb.SaveChangesAsync();

        return Ok();
    }
}
