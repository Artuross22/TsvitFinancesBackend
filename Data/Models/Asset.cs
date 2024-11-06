using Data.Models.Enums;
using Data.Modelsl;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;

public class Asset
{
    public int Id { get; init; }

    public required Guid PublicId { get; set; }

    public required string AppUserId { get; set; } 
    public required AppUser AppUser { get; set; }

    public required Sector Sector { get; set; }

    public InvestmentTerm Term { get; set; }

    public required Market Market { get; set; }

    public required string Name { get; set; }

    public required string Ticker { get; set; }

    public required decimal CurrentPrice { get; set; }

    public required decimal Quantity { get; set; }

    public required decimal InterestOnCurrentDeposit { get; set; }

    public required decimal BoughtFor { get; set; }

    public required DateTime AddedAt { get; set; }

    public required bool IsActive { get; set; }

    public DateTime? ClosedAt { get; set; }

    public decimal? SoldFor { get; set; }

    public required int? StrategyId { get; set; }
    public virtual required Strategy Strategy { get; set; }

    public int? SeasonalityId { get; set; }
    public Seasonality? Seasonalities { get; set; }

    public virtual required IEnumerable<Chart> Charts { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.ToTable("Assets");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Name).IsRequired().HasMaxLength(100);
            builder.Property(e => e.Ticker).IsRequired().HasMaxLength(10);

            builder.HasOne(a => a.AppUser)
                .WithMany(u => u.Assets)
                .HasForeignKey(a => a.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Strategy)
               .WithMany()
               .HasForeignKey(a => a.StrategyId)
               .OnDelete(DeleteBehavior.Restrict);
        }
    }
}