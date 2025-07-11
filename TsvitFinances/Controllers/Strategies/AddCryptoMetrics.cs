using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Strategies;

[Route("api/[controller]")]
[ApiController]
public class AddCryptoMetrics : Controller
{
    readonly protected MainDb _mainDb;

    public AddCryptoMetrics(MainDb mainDb)
    {
        _mainDb = mainDb;
    }

    [HttpPost]
    public async Task<ActionResult> Invoke(BindingModel model)
    {
        var financeData = _mainDb.Set<FinanceData>()
            .AnyAsync(id => id.PublicId == model.FinanceDataId);

        if (financeData == null)
        {
            return NotFound();
        }

        _mainDb.Add(new CryptoMetrics
        {
            PublicId = Guid.NewGuid(),
            MarketCap = model.MarketCap,
            YearHigh = model.YearHigh,
            YearLow = model.YearLow,
            Volume = model.Volume,
            FinanceData = null!,
            FinanceDataId = financeData.Id,
        });

        await _mainDb.SaveChangesAsync();

        return Ok();
    }

    public class BindingModel
    {
        public required Guid FinanceDataId { get; set; }

        public required decimal MarketCap { get; set; }

        public required decimal Volume { get; set; }

        public required decimal YearHigh { get; set; }

        public required decimal YearLow { get; set; }
    }
}
