using Data.Models;
using Data.Modelsl;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Strategies
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AddStockMetrics : Controller
    {
        readonly protected MainDb _mainDb;

        public AddStockMetrics(MainDb mainDb)
        {
            _mainDb = mainDb;
        }

        [HttpPost]
        public async Task<ActionResult> Index(BindingModel model)
        {
            var strategy = await _mainDb.Set<Strategy>()
                .FirstOrDefaultAsync(id => id.FinanceData.PublicId == model.FinanceDataId);

            if (strategy == null)
            {
                return NotFound();
            }

            _mainDb.Add(new StockMetrics
            {
                FinanceDataId = strategy.FinanceDataId!.Value,
                FinanceData = null!,
                PublicId = Guid.NewGuid(),
                PBRatio = model.PBRatio,
                ROA = model.ROA,
                PERatio = model.PERatio,
                ROE = model.ROE,
                EBIT = model.EBIT,
                PSRatio = model.PSRatio,
                NetIncome = model.NetIncome,
                SharesOutstanding = model.SharesOutstanding,
                FreeCashFlow = model.FreeCashFlow,
                DebtRatio = model.DebtRatio,
                RevenueGrowth = model.RevenueGrowth,
                NetProfitMargin = model.NetProfitMargin,
                FreeCashFlowPerShare = model.FreeCashFlowPerShare,
                OperatingCashFlowPerShare = model.OperatingCashFlowPerShare,
                DebtToEquityRatio = model.DebtToEquityRatio,
                DividendYield = model.DividendYield,
            });

            await _mainDb.SaveChangesAsync();

            return Ok();
        }

        public class BindingModel
        {
            public required Guid FinanceDataId { get; set; }

            public required decimal PERatio { get; set; }

            public required decimal OperatingCashFlowPerShare { get; set; }

            public required decimal ROE { get; set; }

            public required decimal PBRatio { get; set; }

            public required decimal DividendYield { get; set; }

            public required decimal DebtToEquityRatio { get; set; }

            public required decimal EBIT { get; set; }

            public required decimal PSRatio { get; set; }

            public required decimal FreeCashFlowPerShare { get; set; }

            public required decimal ROA { get; set; }

            public required decimal NetProfitMargin { get; set; }

            public required decimal RevenueGrowth { get; set; }

            public required decimal DebtRatio { get; set; }

            public required decimal FreeCashFlow { get; set; }

            public required decimal NetIncome { get; set; }

            public required decimal SharesOutstanding { get; set; }
        }
    }
}