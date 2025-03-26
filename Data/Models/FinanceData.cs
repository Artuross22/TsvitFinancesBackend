using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Data.Modelsl;

namespace Data.Models;

public class FinanceData
{
    public int Id { get; set; }

    public required Guid PublicId { get; set; }

    public required int? StrategyId { get; set; }

    public required Strategy Strategy { get; set; }

    public virtual CryptoMetrics? CryptoMetrics { get; set; }
    public virtual StockMetrics? StockMetrics { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<FinanceData>
    {
        public void Configure(EntityTypeBuilder<FinanceData> builder)
        {
            builder.ToTable("FinanceData");

            builder.HasOne(fd => fd.CryptoMetrics)
              .WithOne(fd => fd.FinanceData)
              .HasForeignKey<CryptoMetrics>(cm => cm.FinanceDataId)
              .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(fd => fd.StockMetrics)
                .WithOne(fd => fd.FinanceData)
                .HasForeignKey<StockMetrics>(sm => sm.FinanceDataId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}