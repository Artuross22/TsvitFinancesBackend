using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Assets;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class DeleteAssets : Controller
{
    readonly protected MainDb _mainDb;

    public DeleteAssets(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<Asset>> Index(Guid id)
    {
        var asset = await _mainDb.Set<Asset>()
             .Where(a => a.PublicId == id)
             .SingleOrDefaultAsync();

        if (asset is null)
        {
            return NotFound();
        }

        _mainDb.Remove(asset);
        await _mainDb.SaveChangesAsync();

        return Ok();
    }
}