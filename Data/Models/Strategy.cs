using Data.Models;
using Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Modelsl;

public class Strategy
{
    public int Id { get; init; }

    public required Guid PublicId { get; set; }

    public required string Description { get; set; }

    public required string Name { get; set; }

    public required RiskCategory Category { get; set; }

    public required string AppUserId { get; set; }
    public virtual required AppUser AppUser { get; set; }

    public required int? RiskManagementId { get; set; }
    public virtual required RiskManagement RiskManagement { get; set; }

    public required int? PositionManagementId { get; set; }
    public virtual required PositionManagement PositionManagement { get; set; }

    public required int? FinanceDataId { get; set; }
    public virtual required FinanceData FinanceData { get; set; }

    public virtual ICollection<Asset>? Assets { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<Strategy>
    {
        public void Configure(EntityTypeBuilder<Strategy> builder)
        {
            builder.ToTable("Strategies");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

            builder.HasOne(s => s.AppUser)
                .WithMany(u => u.Strategies)
                .HasForeignKey(s => s.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(s => s.Assets)
                .WithOne(a => a.Strategy)
                .HasForeignKey(a => a.StrategyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}