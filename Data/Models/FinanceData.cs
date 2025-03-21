using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public class FinanceData
{
    public int Id { get; set; }

    public required Guid PublicId { get; set; }

    public virtual CryptoMetrics? CryptoMetrics { get; set; }
    public virtual StockMetrics? StockMetrics { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<FinanceData>
    {
        public void Configure(EntityTypeBuilder<FinanceData> builder)
        {
            builder.ToTable("FinanceData");
        }
    }
}