using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Assets;


[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class ViewAsset : Controller
{
    readonly protected MainDb _mainDb;

    public ViewAsset(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BindingModel>> Index(Guid id)
    {
        var asset = await _mainDb.Set<Asset>()
            .Include(c => c.Charts)
            .Include(c => c.Strategy)
            .FirstOrDefaultAsync(a => a.PublicId == id);

        if (asset == null)
        {
            return NotFound();
        }

        var output = new BindingModel
        {
            PublicId = asset.PublicId,
            StrategyPublicId = asset.Strategy?.PublicId,
            StrategyName = asset.Strategy?.Name,
            AddedAt = asset.AddedAt,
            BoughtFor = asset.BoughtFor,
            CurrentPrice = asset.CurrentPrice,
            InterestOnCurrentDeposit = asset.InterestOnCurrentDeposit,
            Market = asset.Market.ToString(),
            Sector = asset.Sector.ToString(),
            Term = asset.Term.ToString(),
            Name = asset.Name,
            Ticker = asset.Ticker,
            Quantity = asset.Quantity,
        };

        foreach (var item in asset.Charts)
        {
            output.Charts?.Add(new BindingModel._Chart
            {
                Name = item.FileName,
                Description = item.Description,
                ChartsPath = item.FilePath.Substring(item.FilePath.IndexOf("public") + "public".Length).Replace("\\", "/"),
            });
        }


        if (asset is null)
        {
            return NotFound();
        }

        return output;
    }

    public class BindingModel
    {
        public required Guid PublicId { get; set; }

        public required string Sector { get; set; }

        public required string Term { get; set; }

        public required string Market { get; set; }

        public required string Name { get; set; }

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
