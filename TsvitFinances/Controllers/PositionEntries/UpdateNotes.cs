using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.PositionEntries;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class UpdateNotes : Controller
{
    readonly protected MainDb _mainDb;

    public UpdateNotes(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpPut]
    public async Task<IActionResult> Index(BindingModel model)
    {
        var positionEntryNote = await _mainDb.Set<PositionEntryNote>()
            .FirstOrDefaultAsync(c => c.PublicId == model.Id);

        if (positionEntryNote is null)
        {
            return NotFound();
        }
        positionEntryNote.Note = model.Note;

        await _mainDb.SaveChangesAsync();

        return Ok();
    }

    public class BindingModel
    {
        public required Guid Id { get; set; }
        public required string Note { get; set; }
    }
}