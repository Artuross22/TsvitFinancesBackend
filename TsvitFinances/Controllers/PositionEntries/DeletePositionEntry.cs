using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.DeletePositionEntry;

[Route("api/[controller]")]
[ApiController]
public class DeletePositionEntry : Controller
{
    readonly protected MainDb _mainDb;

    public DeletePositionEntry(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpDelete("{publicId}")]
    public async Task<IActionResult> Invoke(Guid publicId)
    {
        var positionEntryNote = await _mainDb.Set<PositionEntryNote>()
            .Where(pen => pen.PublicId == publicId)
            .FirstOrDefaultAsync();

        if (positionEntryNote is null)
        {
            return NotFound();
        }

        _mainDb.Remove(positionEntryNote);

        await _mainDb.SaveChangesAsync();

        return Ok();
    }
}