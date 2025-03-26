using Data;
using Data.Models;
using Data.Models.Enums;
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

    [HttpGet("{publicId}/{assetPublicId}")]
    public async Task<ActionResult> Index(Guid publicId, Guid assetPublicId)
    {
        var data = await _mainDb.Set<FinanceData>()
            .Include(f => f.StockMetrics)
            .FirstOrDefaultAsync(fd => fd.Strategy.PublicId == publicId);

        await _mainDb.SaveChangesAsync();

        if (data == null)
        {
            return NotFound();
        }

        var asset = await _mainDb.Set<Asset>()
            .Where(a => a.PublicId == assetPublicId)
            .SingleAsync();

        if (asset.Market != Market.Stock)
        {
            return Ok();
        }

        var shareDatas = (await _connection.GetShare(asset.Ticker)).First();


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
            RecommendedDebtRatio = data.StockMetrics.DebtRatio,

            PSRatio = shareDatas.PSRatio,
            RecommendedPSRatio = data.StockMetrics.PSRatio,

            PBRatio = shareDatas.PBRatio,
            RecommendedPBRatio = data.StockMetrics.PBRatio,

            PERatio = shareDatas.PERatio,
            RecommendedPERatio = data.StockMetrics.PERatio,

            ROA = shareDatas.ROA,
            RecommendedROA = data.StockMetrics.ROA,

            ROE = shareDatas.ROE,
            RecommendedROE = data.StockMetrics.ROE,

            EBIT = shareDatas.EBIT,
            RecommendedEBIT = data.StockMetrics.EBIT,

            FreeCashFlow = shareDatas.FreeCashFlow,
            RecommendedFreeCashFlow = data.StockMetrics.FreeCashFlow,

            NetIncome = shareDatas.NetIncome,
            RecommendedNetIncome = data.StockMetrics.NetIncome,

            NetProfitMargin = shareDatas.NetProfitMargin,
            RecommendedNetProfitMargin = data.StockMetrics.NetProfitMargin,

            DividendYield = shareDatas.DividendYield,
            RecommendedDividendYield = data.StockMetrics.DividendYield,

            DebtToEquityRatio = shareDatas.DebtToEquityRatio,
            RecommendedDebtToEquityRatio = data.StockMetrics.DebtToEquityRatio,

            FreeCashFlowPerShare = shareDatas.FreeCashFlowPerShare,
            RecommendedFreeCashFlowPerShare = data.StockMetrics.FreeCashFlowPerShare,

            OperatingCashFlowPerShare = shareDatas.OperatingCashFlowPerShare,
            RecommendedOperatingCashFlowPerShare = data.StockMetrics.OperatingCashFlowPerShare,
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
    }
}