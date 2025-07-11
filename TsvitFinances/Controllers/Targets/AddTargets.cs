using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Targets;

[Route("api/[controller]")]
[ApiController]
public class AddTargets : Controller
{
    readonly protected MainDb _mainDb;

    public AddTargets(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpPost]
    public async Task<ActionResult> Invoke(AddTarget model)
    {
        var asset = await _mainDb.Set<Asset>()
            .FirstOrDefaultAsync(a => a.PublicId == model.PublicId);

        if (asset == null)
        {
            return NotFound();
        }

        foreach (var item in model.BuyLevels)
        {
            _mainDb.Add(new PurchaseLevel
            {
                PublicId = Guid.NewGuid(),
                Level = item.Level,
                AverageLevel = item.AverageLevel,
                Description = item.Description,
                AssetId = asset.Id,
                Asset = asset
            });
        }

        foreach (var item in model.SalesLevels)
        {
            _mainDb.Add(new SalesLevels
            {
                PublicId = Guid.NewGuid(),
                Level = item.Level,
                AverageLevel = item.AverageLevel,
                Description = item.Description,
                AssetId = asset.Id,
                Asset = asset
            });
        }

        await _mainDb.SaveChangesAsync();

        return Ok();
    }

    public class SaleLevels
    {
        public string? Description { get; set; }
        public required decimal Level { get; set; }
        public required decimal? AverageLevel { get; set; }
    }

    public class BuyLevel
    {
        public string? Description { get; set; }
        public required decimal Level { get; set; }
        public required decimal? AverageLevel { get; set; }
    }

    public class AddTarget
    {
        public required Guid PublicId { get; set; }
        public required ICollection<BuyLevel> BuyLevels { get; set; }
        public required ICollection<SaleLevels> SalesLevels { get; set; }
    }
}