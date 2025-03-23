using Data;
using Data.Models;
using Data.Modelsl;
using FinancialData.APIs.FPM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TsvitFinances.Controllers.Strategies;

[AllowAnonymous]
[Route("api/[controller]")]
[ApiController]
public class ApplyStockMetrics : Controller
{
    readonly protected MainDb _mainDb;
    protected readonly FpmConnection _connection;

    public ApplyStockMetrics(MainDb mainDb, FpmConnection fpmConnection)
    {
        _mainDb = mainDb;
        _connection = fpmConnection;
    }

    [HttpGet("{publicId}/{symbol}")]
    public async Task<ActionResult> Index(Guid publicId, string symbol)
    {
        var data = await _mainDb.Set<Strategy>()
            .Include(f => f.FinanceData.StockMetrics)
            .FirstOrDefaultAsync(fd => fd.PublicId == publicId);

        await _mainDb.SaveChangesAsync();

        if (data == null)
        {
            return NotFound();
        }

        var shareDatas = (await _connection.GetShare(symbol)).First();

        if (shareDatas == null)
        {
            return BadRequest();
        }

        var modelPost = new BindingModel
        {
            FinanceDataId = data.PublicId,
            Symbol = shareDatas.Symbol,
            Date = shareDatas.Date,

            DebtRatio = shareDatas.DebtRatio,
            RecommendedDebtRatio = data.FinanceData.StockMetrics.DebtRatio,

            PSRatio = shareDatas.PSRatio,
            RecommendedPSRatio = data.FinanceData.StockMetrics.PSRatio,

            PBRatio = shareDatas.PBRatio,
            RecommendedPBRatio = data.FinanceData.StockMetrics.PBRatio,

            PERatio = shareDatas.PERatio,
            RecommendedPERatio = data.FinanceData.StockMetrics.PERatio,

            ROA = shareDatas.ROA,
            RecommendedROA = data.FinanceData.StockMetrics.ROA,

            ROE = shareDatas.ROE,
            RecommendedROE = data.FinanceData.StockMetrics.ROE,

            EBIT = shareDatas.EBIT,
            RecommendedEBIT = data.FinanceData.StockMetrics.EBIT,

            FreeCashFlow = shareDatas.FreeCashFlow,
            RecommendedFreeCashFlow = data.FinanceData.StockMetrics.FreeCashFlow,

            NetIncome = shareDatas.NetIncome,
            RecommendedNetIncome = data.FinanceData.StockMetrics.NetIncome,

            NetProfitMargin = shareDatas.NetProfitMargin,
            RecommendedNetProfitMargin = data.FinanceData.StockMetrics.NetProfitMargin,

            DividendYield = shareDatas.DividendYield,
            RecommendedDividendYield = data.FinanceData.StockMetrics.DividendYield,

            RevenueGrowth = shareDatas.RevenueGrowth,
            RecommendedRevenueGrowth = data.FinanceData.StockMetrics.RevenueGrowth,

            SharesOutstanding = shareDatas.SharesOutstanding,
            RecommendedSharesOutstanding = data.FinanceData.StockMetrics.SharesOutstanding,

            DebtToEquityRatio = shareDatas.DebtToEquityRatio,
            RecommendedDebtToEquityRatio = data.FinanceData.StockMetrics.DebtToEquityRatio,

            FreeCashFlowPerShare = shareDatas.FreeCashFlowPerShare,
            RecommendedFreeCashFlowPerShare = data.FinanceData.StockMetrics.FreeCashFlowPerShare,

            OperatingCashFlowPerShare = shareDatas.OperatingCashFlowPerShare,
            RecommendedOperatingCashFlowPerShare = data.FinanceData.StockMetrics.OperatingCashFlowPerShare,
        };

        return Ok(modelPost);
    }

    public class BindingModel
    {
        public required Guid FinanceDataId { get; set; }
        public required string Symbol { get; set; }
        public required string Date { get; set; }

        public required decimal PERatio { get; set; }
        public required decimal RecommendedPERatio { get; set; }
        public required decimal PBRatio { get; set; }
        public required decimal RecommendedPBRatio { get; set; }
        public required decimal PSRatio { get; set; }
        public required decimal RecommendedPSRatio { get; set; }
        public required decimal DividendYield { get; set; }
        public required decimal RecommendedDividendYield { get; set; }
        public required decimal DebtToEquityRatio { get; set; }
        public required decimal RecommendedDebtToEquityRatio { get; set; }
        public required decimal DebtRatio { get; set; }
        public required decimal RecommendedDebtRatio { get; set; }

        public required decimal ROE { get; set; }
        public required decimal RecommendedROE { get; set; }
        public required decimal ROA { get; set; }
        public required decimal RecommendedROA { get; set; }
        public required decimal NetProfitMargin { get; set; }
        public required decimal RecommendedNetProfitMargin { get; set; }
        public required decimal RevenueGrowth { get; set; }
        public required decimal RecommendedRevenueGrowth { get; set; }

        public required decimal EBIT { get; set; }
        public required decimal RecommendedEBIT { get; set; }
        public required decimal OperatingCashFlowPerShare { get; set; }
        public required decimal RecommendedOperatingCashFlowPerShare { get; set; }
        public required decimal FreeCashFlowPerShare { get; set; }
        public required decimal RecommendedFreeCashFlowPerShare { get; set; }
        public required decimal FreeCashFlow { get; set; }
        public required decimal RecommendedFreeCashFlow { get; set; }

        public required decimal NetIncome { get; set; }
        public required decimal RecommendedNetIncome { get; set; }
        public required decimal SharesOutstanding { get; set; }
        public required decimal RecommendedSharesOutstanding { get; set; }
        //public required decimal MarketCapitalization { get; set; }
        //public required decimal RecommendedMarketCapitalization { get; set; }
        //public required decimal EnterpriseValue { get; set; }
        //public required decimal RecommendedEnterpriseValue { get; set; }
        //public required decimal CurrentRatio { get; set; }
        //public required decimal RecommendedCurrentRatio { get; set; }
        //public required decimal QuickRatio { get; set; }
        //public required decimal RecommendedQuickRatio { get; set; }
        //public required decimal EarningsYield { get; set; }
        //public required decimal RecommendedEarningsYield { get; set; }
    }
}