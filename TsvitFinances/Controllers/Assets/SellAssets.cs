using Data;
using Data.Models;
using Data.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Assets;

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
    public async Task<IActionResult> Invoke(Guid id)
    {
        var asset = await _mainDb.Set<Asset>()
             .Include(a => a.AppUser)
             .Where(a => a.PublicId == id)
             .SingleOrDefaultAsync();

        if (asset is null)
        {
            return NotFound();
        }

        if(!asset.IsActive)
        {
            return Ok();
        }

        var now = DateTime.UtcNow;

        var profit = (asset.CurrentPrice - asset.BoughtFor) * asset.Quantity;

        _mainDb.Add(new BalanceFlow
        {
            AppUser = asset.AppUser,
            AppUserId = asset.AppUserId,
            Sum = profit,
            CreatedOn = now,
            Balance = Balance.NetInternalIncome, 
        });

        var total = asset.CurrentPrice * asset.Quantity;

        _mainDb.Add(new BalanceFlow
        {
            AppUser = asset.AppUser,
            AppUserId = asset.AppUserId,
            Sum = total,
            CreatedOn = now,
            Balance = Balance.Total,
        });

        asset.SoldFor = asset.CurrentPrice;
        asset.ClosedAt = now;
        asset.IsActive = false;

        await _mainDb.SaveChangesAsync();

        return Ok();
    }
}
