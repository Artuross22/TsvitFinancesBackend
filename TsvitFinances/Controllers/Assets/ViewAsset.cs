using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

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
          .Include(a => a.PositionEntryNotes)
              .ThenInclude(n => n.Charts)
          .Include(a => a.Strategy)
          .FirstOrDefaultAsync(a => a.PublicId == id);

        if (asset == null)
        {
            return NotFound();
        }

        var percentageProfit = (asset.CurrentPrice - asset.BoughtFor) / asset.BoughtFor * 100;
        var profit = (asset.CurrentPrice - asset.BoughtFor) * asset.Quantity;

        var output = new BindingModel
        {
            PublicId = asset.PublicId,
            StrategyPublicId = asset.Strategy?.PublicId,
            StrategyName = asset.Strategy?.Name,
            AddedAt = asset.AddedAt,
            Goal = asset.Goal,
            BoughtFor = asset.BoughtFor,
            CurrentPrice = asset.CurrentPrice,
            InterestOnCurrentDeposit = asset.InterestOnCurrentDeposit,
            Market = asset.Market.ToString(),
            Sector = asset.Sector.ToString(),
            Term = asset.Term.ToString(),
            PercentageProfit = Math.Round(percentageProfit, 2),
            Profit = Math.Round(profit, 2),
            Name = asset.Name,
            Ticker = asset.Ticker,
            Quantity = asset.Quantity,
            
        };

        var chartsToAdd = asset.PositionEntryNotes
            .Where(note => note.Charts != null)
            .SelectMany(note => note.Charts!)
            .Select(chart => new BindingModel._Chart
            {
                Name = chart.FileName,
                Description = chart.Description,
                ChartsPath = _getPublicPath(chart.FilePath)
            });

        output.Charts.AddRange(chartsToAdd);

         
        if (asset is null)
        {
            return NotFound();
        }

        return output;
    }

    private static string _getPublicPath(string filePath)
    {
        const string PUBLIC_MARKER = "public";

        var publicIndex = filePath.IndexOf(PUBLIC_MARKER, StringComparison.OrdinalIgnoreCase);
        return publicIndex >= 0
            ? filePath.Substring(publicIndex + PUBLIC_MARKER.Length).Replace("\\", "/")
            : filePath;
    }

    public class BindingModel
    {
        public required Guid PublicId { get; set; }

        public required string Sector { get; set; }

        public required string Term { get; set; }

        public required string Market { get; set; }

        public required string Goal { get; set; }

        public required string Name { get; set; }

        public required string Ticker { get; set; }

        public required decimal CurrentPrice { get; set; }

        public required decimal Quantity { get; set; }

        public required decimal PercentageProfit { get; set; }

        public required decimal Profit { get; set; }

        public required decimal BoughtFor { get; set; }

        public required Guid? StrategyPublicId { get; set; }

        public required string? StrategyName { get; set; }

        public required DateTime AddedAt { get; set; }

        public required decimal InterestOnCurrentDeposit { get; set; }

        public IList<_Chart>? Charts { get; set; } = [];

        public class _Chart
        {
            public string Name { get; set; } = string.Empty;

            public string? Description { get; set; } = string.Empty;

            public string ChartsPath { get; set; } = string.Empty;
        }
    }
}