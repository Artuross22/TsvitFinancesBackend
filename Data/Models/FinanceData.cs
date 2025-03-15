using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public class FinanceData
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public int CryptoMetricsId { get; set; }
    public required CryptoMetrics? CryptoMetrics { get; set;}

    public int StockMetricsId { get; set; }
    public required StockMetrics? StockMetrics { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<FinanceData>
    {
        public void Configure(EntityTypeBuilder<FinanceData> builder)
        {
            builder.ToTable("FinanceData");

            builder.HasMany(s => s.st)
                .WithOne(a => a.Strategy)
                .HasForeignKey(a => a.StrategyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}