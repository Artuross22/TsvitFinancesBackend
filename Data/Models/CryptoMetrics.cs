using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Models
{
    public class CryptoMetrics
    {
        public int Id { get; set; }

        public required Guid PublicId { get; set; }

        public required decimal MarketCap { get; set; }

        public required decimal Volume { get; set; }

        public required decimal YearHigh { get; set; }

        public required decimal YearLow { get; set; }

        public required int FinanceDataId { get; set; }
        public required FinanceData FinanceData { get; set; }

        internal class EFConfiguration : IEntityTypeConfiguration<CryptoMetrics>
        {
            public void Configure(EntityTypeBuilder<CryptoMetrics> builder)
            {
                builder.ToTable("CryptoMetrics");
            }
        }
    }
}