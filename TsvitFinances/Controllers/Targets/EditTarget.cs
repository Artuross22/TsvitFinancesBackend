using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Targets;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class EditTarget : Controller
{
    readonly protected MainDb _mainDb;

    public EditTarget(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet("{publicId}/{levelName}")]
    public async Task<ActionResult<BindingModelGet>> Index(Guid publicId, string levelName)
    {
        if (string.Equals(levelName, "SaleLevels", StringComparison.OrdinalIgnoreCase))
        {
            var salesLevels = await _mainDb.Set<SalesLevels>()
                .Select(b => new BindingModelGet
                {
                    PublicId = b.PublicId,
                    Description = b.Description,
                    Level = b.Level,
                    AverageLevel = b.AverageLevel
                })
                .FirstOrDefaultAsync(a => a.PublicId == publicId);

            if (salesLevels is null)
            {
                return NotFound();
            }

            return salesLevels;
        }
        else if (string.Equals(levelName, "PurchaseLevels", StringComparison.OrdinalIgnoreCase))
        {
            var purchaseLevel = await _mainDb.Set<PurchaseLevel>()
            .Select(b => new BindingModelGet
            {
                PublicId = b.PublicId,
                Description = b.Description,
                Level = b.Level,
                AverageLevel = b.AverageLevel
            })
            .FirstOrDefaultAsync(a => a.PublicId == publicId);

            if (purchaseLevel is null)
            {
                return NotFound();
            }

            return purchaseLevel;
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> Index(BindingModelPost model)
    {
        if (string.Equals(model.levelName, "SaleLevels", StringComparison.OrdinalIgnoreCase))
        {
            var saleLevels = await _mainDb.Set<SalesLevels>()
                .FirstOrDefaultAsync(a => a.PublicId == model.PublicId);

            if (saleLevels == null)
            {
                return NotFound();
            }

            saleLevels.Level = model.Level;
            saleLevels.AverageLevel = model.AverageLevel;
            saleLevels.Description = model.Description;
        }
        else if (string.Equals(model.levelName, "PurchaseLevels", StringComparison.OrdinalIgnoreCase))
        {
            var purchaseLevel = await _mainDb.Set<PurchaseLevel>()
                .FirstOrDefaultAsync(a => a.PublicId == model.PublicId);

            if (purchaseLevel is null)
            {
                return NotFound();
            }

            purchaseLevel.Level = model.Level;
            purchaseLevel.AverageLevel = model.AverageLevel;
            purchaseLevel.Description = model.Description;
        }

        await _mainDb.SaveChangesAsync();

        return Ok();
    }

    public class BindingModelGet
    {
        public Guid PublicId { get; set; }
        public string? Description { get; set; }
        public required decimal Level { get; set; }
        public required decimal? AverageLevel { get; set; }
    }

    public class BindingModelPost
    {
        public required Guid PublicId { get; set; }
        public required string levelName { get; set; }
        public required decimal Level { get; set; }
        public required decimal? AverageLevel { get; set; }
        public required string? Description { get; set; }
    }
}