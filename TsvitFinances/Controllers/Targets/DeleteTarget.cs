using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Targets;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class DeleteTarget : Controller
{
    readonly protected MainDb _mainDb;

    public DeleteTarget(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpDelete("{publicId}/{levelName}")]
    public async Task<IActionResult> Invoke(Guid publicId, string levelName)
    {

        if (string.Equals(levelName, "SaleLevels", StringComparison.Ordinal))
        {
            var level = await _mainDb.Set<SalesLevels>()
                .SingleAsync(c => c.PublicId == publicId);

            _mainDb.Remove(level);
        }
        else if (string.Equals(levelName, "PurchaseLevels", StringComparison.Ordinal))
        {
            var level = await _mainDb.Set<PurchaseLevel>()
                .SingleAsync(c => c.PublicId == publicId);

            _mainDb.Remove(level);
        }

        await _mainDb.SaveChangesAsync();

        return Ok();
    }
}