using Data.Models;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace TsvitFinances.Controllers.Assets;


[AllowAnonymous]
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
    public async Task<IActionResult> Index(int id, Guid assetId)
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
