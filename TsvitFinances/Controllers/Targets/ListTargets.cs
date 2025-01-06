using Data;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Targets;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class ListTargets : Controller
{
    readonly protected MainDb _mainDb;

    public ListTargets(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpGet("{publicId}")]
    public async Task<ActionResult<BindingModel>> Index(Guid publicId)
    {
        var listTargets = await _mainDb.Set<Asset>()
            .Where(s => s.PublicId == publicId)
            .Select(s => new BindingModel
            {
                PublicId = s.PublicId,
                BuyLevels = s.PurchaseLevels.Select(bl => new BuyLevel
                {
                    PublicId = bl.PublicId,
                    Description = bl.Description,
                    Level = bl.Level,
                    AverageLevel = bl.AverageLevel
                })
                .ToList(),

                SalesLevels = s.SalesLevels.Select(sl => new SaleLevels
                {
                    PublicId = sl.PublicId,
                    Description = sl.Description,
                    Level = sl.Level,
                    AverageLevel = sl.AverageLevel
                })
                .ToList()
            })
            .FirstOrDefaultAsync();

        if (listTargets == null)
        {
            return NotFound();
        }

        return listTargets;
    }

    public class SaleLevels
    {
        public Guid PublicId { get; set; }
        public string? Description { get; set; }
        public required decimal Level { get; set; }
        public required decimal? AverageLevel { get; set; }
    }

    public class BuyLevel
    {
        public Guid PublicId { get; set; }
        public string? Description { get; set; }
        public required decimal Level { get; set; }
        public required decimal? AverageLevel { get; set; }
    }

    public class BindingModel
    {
        public required Guid PublicId { get; set; }
        public required List<BuyLevel> BuyLevels { get; set; }
        public required List<SaleLevels> SalesLevels { get; set; }
    }
}