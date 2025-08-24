using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.InvestmentIdeas;

[Route("api/[controller]")]
[ApiController]
public class AddInvestmentIdea : Controller
{
    protected readonly MainDb _mainDb;
    public AddInvestmentIdea(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Invoke(string id)
    {
        var assets = await _mainDb.Set<Asset>()
            .Where(a => a.AppUserId == id)
            .Select(a => new AssetModel
            {
                PublicId = a.PublicId,
                Name = a.Name
            })
            .ToListAsync();

        if (assets == null)
        {
            return NotFound();
        }

        return Ok(assets);
    }

    [HttpPost]
    public async Task<IActionResult> Index(BindingModel model)
    {
        var user = await _mainDb.Set<AppUser>()
            .Where(a => a.Id == model.AppUserId)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound();
        }

        var investmentIdea = new InvestmentIdea
        {
            PublicId = Guid.NewGuid(),
            Name = model.Name,
            Description = model.Description,
            ExpectedReturn = model.ExpectedReturn,
            Profit = model.Profit,
            CreatedAt = DateTime.UtcNow,
            ClosedAt = null!,
            AppUserId = null!,
            AppUser = user,
            Assets = null!
        };

        if (model.Assets != null)
        {
            investmentIdea.Assets = await _mainDb.Set<Asset>()
                .Where(a => model.Assets.Select(a => a.PublicId).Contains(a.PublicId))
                .ToListAsync();
        }

        _mainDb.Add(investmentIdea);

        await _mainDb.SaveChangesAsync();

        return Ok(investmentIdea.PublicId);
    }

    public class AssetModel
    {
        public Guid PublicId { get; set; }
        public string Name { get; set; }
    }

    public class BindingModel
    {
        public required string AppUserId { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal ExpectedReturn { get; set; }
        public decimal? Profit { get; set; }
        public IList<_Asset>? Assets { get; set; }
    }

    public class _Asset
    {
        public required Guid PublicId { get; set; }

        public required string Name { get; set; }
    }
}