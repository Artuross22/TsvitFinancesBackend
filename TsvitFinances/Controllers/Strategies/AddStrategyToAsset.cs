using Data;
using Data.Models;
using Data.Modelsl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Strategies;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class AddStrategyToAsset : Controller
{
    readonly protected MainDb _mainDb;
    public AddStrategyToAsset(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet("{userId}/{assetPublicId}")]
    public async Task<IActionResult> Index(string userId, Guid assetPublicId)
    {
        var strategies = await _mainDb.Set<Strategy>()
            .Where(s => s.AppUser.Id == userId)
            .Select(s => new ListStrategies
            {
                PublicId = s.PublicId,
                Name = s.Name,
                IsSetToCurrentAsset = s.Assets != null
                    ? s.Assets.Any(a => a.PublicId == assetPublicId)
                    : false,
            })
            .ToListAsync();

        if (strategies == null)
        {
            return NotFound();
        }

        return Json(strategies);
    }

    [HttpPost]
    public async Task<IActionResult> Index(BindingModel model)
    {
        var strategy = await _mainDb.Set<Strategy>()
            .FirstOrDefaultAsync(s => s.PublicId == model.StrategyPublicId);

        var asset = await _mainDb.Set<Asset>()
            .FirstOrDefaultAsync(s => s.PublicId == model.AssetPublicId);

        if (strategy == null || asset == null)
        {
            return NotFound();
        }

        asset.Strategy = strategy;
        await _mainDb.SaveChangesAsync();

        return Ok();
    }

    public class ListStrategies
    {
        public required Guid PublicId { get; set; }
        public required string Name { get; set; }
        public required bool IsSetToCurrentAsset { get; set; }
    }

    public class BindingModel
    {
        public Guid AssetPublicId { get; set; }
        public Guid StrategyPublicId { get; set; }
    }
}