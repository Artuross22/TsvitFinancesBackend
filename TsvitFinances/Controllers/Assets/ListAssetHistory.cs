using Data;
using Data.Models;
using Data.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Assets;

[Route("api/[controller]")]
[ApiController]
public class ListAssetHistory : Controller
{
    readonly protected MainDb _mainDb;
    private readonly ILogger<AddAssetHistory> _logger;

    public ListAssetHistory(MainDb mainDb, ILogger<AddAssetHistory> logger)
    {
        _mainDb = mainDb;
        _logger = logger;
    }

    [HttpGet("{assetPublicId}")]
    public async Task<ActionResult> Invoke(Guid assetPublicId)
    {
        var assets = await _mainDb.Set<AssetHistory>()
            .Where(ah => ah.Asset.PublicId == assetPublicId)
            .Select(ah => new BindingModel
            {
                PublicId = ah.PublicId,
                Price = ah.Price,
                Quantity = ah.Quantity,
                Type = ah.Type,
            })
            .ToListAsync();

        if (assets == null)
        {
            return NotFound();
        }

        return Ok(assets);
    }

    public class BindingModel
    {
        public required Guid PublicId { get; set; }
        public required decimal Quantity { get; set; }
        public required decimal Price { get; set; }
        public required PositionType Type { get; set; }
    }
}