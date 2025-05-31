using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Assets;


[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class UpdateAssets : Controller
{
    readonly protected MainDb _mainDb;

    public UpdateAssets(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetAssetsDto>> Index(Guid id)
    {
        var asset = await _mainDb.Set<Asset>()
            .Include(c => c.AppUser)
            .FirstOrDefaultAsync(a => a.PublicId == id);

        if (asset == null)
        {
            return NotFound();
        }

        var output = new AssetUpdateDto
        {
            PublicId = asset.PublicId,
            UserPublicId = asset.AppUser.Id,
            Goal = asset.Goal,
            BoughtFor = asset.BoughtFor,
            CurrentPrice = asset.CurrentPrice,
            Quantity = asset.Quantity,
            Name = asset.Name,
            Ticker = asset.Ticker,
        };

        return Json(output);
    }

    [HttpPut]
    public async Task<ActionResult<Asset>> Index(AssetUpdateDto model)
    {
        var user = await _mainDb.Users.SingleAsync(u => u.Id == model.UserPublicId);

        if (user == null)
        {
            return NotFound();
        }

        var asset = await _mainDb.Set<Asset>()
             .Where(a => a.PublicId == model.PublicId)
             .SingleOrDefaultAsync();

        if (asset is null)
        {
            return NotFound();
        }

        asset.Name = model.Name;
        asset.Ticker = model.Ticker;
        asset.CurrentPrice = model.CurrentPrice;
        asset.BoughtFor = model.BoughtFor;
        asset.Quantity = model.Quantity;
        asset.Goal = model.Goal;

        await _mainDb.SaveChangesAsync();

        return Ok();
    }

    public class AssetUpdateDto
    {
        public Guid PublicId { get; set; }

        public required string UserPublicId { get; set; }

        public required string Name { get; set; }

        public required string Goal { get; set; }

        public required string Ticker { get; set; }

        public required decimal Quantity { get; set; }

        public decimal CurrentPrice { get; set; }

        public decimal BoughtFor { get; set; }
    }

    public class GetAssetsDto
    {
        public required Guid PublicId { get; set; }

        public required string Sector { get; set; }

        public required string Term { get; set; }

        public required string Market { get; set; }

        public required string Name { get; set; }

        public required string Goal { get; set; }

        public required string Ticker { get; set; }

        public required decimal CurrentPrice { get; set; }

        public required decimal Quantity { get; set; }

        public required decimal BoughtFor { get; set; }

        public required Guid? StrategyPublicId { get; set; }

        public required string? StrategyName { get; set; }

        public required DateTime AddedAt { get; set; }

        public required decimal InterestOnCurrentDeposit { get; set; }

        public IList<_Chart>? Charts { get; set; } = [];

        public class _Chart
        {
            public string Name { get; set; } = string.Empty;

            public string Description { get; set; } = string.Empty;

            public string ChartsPath { get; set; } = string.Empty;
        }
    }
}
