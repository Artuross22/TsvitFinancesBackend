using Data;
using Data.Models;
using Data.Modelsl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Strategies
{
    [AllowAnonymous]
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
        public async Task<ActionResult> Index(BindingModel model)
        {
            var strategy = _mainDb.Set<Strategy>()
                .AnyAsync(id => id.FinanceDataId == model.FinanceDataId);

            if (strategy == null)
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
                FinanceDataId = model.FinanceDataId,
            });

            await _mainDb.SaveChangesAsync();

            return Ok();
        }

        public class BindingModel
        {
            public required int FinanceDataId { get; set; }

            public required decimal MarketCap { get; set; }

            public required decimal Volume { get; set; }

            public required decimal YearHigh { get; set; }

            public required decimal YearLow { get; set; }
        }
    }
}
