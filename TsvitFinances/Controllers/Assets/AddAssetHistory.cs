using Data;
using Data.Models;
using Data.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Assets;

[Route("api/[controller]")]
[ApiController]
public class AddAssetHistory : Controller
{
    readonly protected MainDb _mainDb;
    private readonly ILogger<AddAssetHistory> _logger;

    public AddAssetHistory(MainDb mainDb, ILogger<AddAssetHistory> logger)
    {
        _mainDb = mainDb;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult> Invoke(BindingModel model)
    {
        var asset = await _mainDb.Set<Asset>()
            .SingleAsync(u => u.PublicId == model.AssetId);

        if (asset == null)
        {
            return NotFound();
        }

        if (model.Type == PositionType.Long)
        {
            asset.Buy(model.Quantity, model.Price);
        }
        else if (model.Type == PositionType.Short)
        {
            asset.Sell(model.Quantity, model.Price);
        }
        else
        {
            _logger.LogError("Invalid PositionType {PositionType} provided for AssetId {AssetId}.", model.Type, model.AssetId);
        }

        await _mainDb.SaveChangesAsync();

        return Ok();
    }

    public class BindingModel
    {
        public required Guid AssetId { get; set; }
        public required decimal Quantity { get; set; }
        public required decimal Price { get; set; }
        public required PositionType Type { get; set; }
    }
}