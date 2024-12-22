using Data.Models.Enums;
using Data.Models;
using Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace TsvitFinances.Controllers.Assets;


[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class SellAssets : Controller
{
    readonly protected MainDb _mainDb;

    public SellAssets(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> Index(Guid id)
    {
        var asset = await _mainDb.Set<Asset>()
             .Include(a => a.AppUser)
             .Where(a => a.PublicId == id)
             .SingleOrDefaultAsync();

        if (asset is null)
        {
            return NotFound();
        }

        var now = DateTime.UtcNow;

        asset.SoldFor = asset.CurrentPrice;
        asset.ClosedAt = now;
        asset.IsActive = false;

        _mainDb.Add(new BalanceFlow
        {
            AppUser = asset.AppUser,
            AppUserId = asset.AppUserId,
            Sum = asset.CurrentPrice,
            CreatedOn = now,
            Balance = Balance.InternalRevenue,
        });

        await _mainDb.SaveChangesAsync();

        return Ok();
    }
}
