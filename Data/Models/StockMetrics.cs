using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;

public class StockMetrics
{
    public int Id { get; set; }

    public required Guid PublicId { get; set; }

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

    internal class EFConfiguration : IEntityTypeConfiguration<StockMetrics>
    {
        public void Configure(EntityTypeBuilder<StockMetrics> builder)
        {
            builder.ToTable("StockMetrics");
        }
    }
}