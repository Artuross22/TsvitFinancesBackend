using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace TsvitFinances.Controllers.InvestmentIdeas;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class EditInvestmentIdea : Controller
{
    protected readonly MainDb _mainDb;

    public EditInvestmentIdea(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet("{publicId}")]
    public async Task<IActionResult> Index(Guid publicId)
    {
        var investmentIdea = await _mainDb.Set<InvestmentIdea>()
            .Where(ii => ii.PublicId == publicId)
            .Select(ii => new BindingModel
            {
                PublicId = ii.PublicId,
                Name = ii.Name,
                Description = ii.Description,
                ExpectedReturn = ii.ExpectedReturn,
                Profit = ii.Profit,
                Assets = ii.Assets.Select(a => new _Asset
                {
                    PublicId = a.PublicId,
                    Name = a.Name
                })
                .ToList()

            })
            .FirstOrDefaultAsync();

        if (investmentIdea == null)
        {
            return NotFound();
        }

        return Ok(investmentIdea);
    }

    [HttpPost]
    public async Task<IActionResult> Index(BindingModel model)
    {
        var investmentIdea = await _mainDb.Set<InvestmentIdea>()
            .Include(ii => ii.Assets)
            .Where(ii => ii.PublicId == model.PublicId)
            .FirstOrDefaultAsync();

        if (investmentIdea == null)
        {
            return NotFound();
        }

        var assets = await _mainDb.Set<Asset>()
            .Where(a => a.AppUserId == investmentIdea.AppUserId)
            .ToListAsync();

        investmentIdea.Name = model.Name;
        investmentIdea.Description = model.Description;
        investmentIdea.ExpectedReturn = model.ExpectedReturn;
        investmentIdea.Profit = model.Profit;

        var investmentIdeaAssetIds = investmentIdea.Assets.Select(a => a.PublicId);

        foreach (var asset in model.Assets)
        {
            if (!investmentIdeaAssetIds.Contains(asset.PublicId))
            {
                var newAsset = assets.First(a => a.PublicId == asset.PublicId);
                investmentIdea.Assets.Add(newAsset);
            }
        }

        var modelAssetIds = model.Assets.Select(a => a.PublicId);

        var removeAssets = investmentIdea.Assets.Where(a => !modelAssetIds.Contains(a.PublicId));
        _mainDb.RemoveRange(removeAssets);

        await _mainDb.SaveChangesAsync();

        return Ok();
    }

    public class BindingModel
    {
        public required Guid PublicId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal ExpectedReturn { get; set; }
        public decimal? Profit { get; set; }
        public IList<_Asset> Assets { get; set; } = [];
    }

    public class _Asset
    {
        public required string Name { get; set; }
        public required Guid PublicId { get; set; }
    }
}