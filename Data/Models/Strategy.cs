using Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Modelsl;

public class Strategy
{
    public int Id { get; init; }

    public Guid PublicId { get; set; }

    public required string Name { get; set; }

    public required RiskManagement RiskManagement { get; set; }

    public required PositionManagement PositionManagement { get; set; }

    public virtual required ICollection<Asset> Assets { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<Strategy>
    {
        public void Configure(EntityTypeBuilder<Strategy> builder)
        {
            builder.ToTable("Strategies");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.HasMany(s => s.Assets)
                .WithOne(a => a.Strategy)
                .HasForeignKey(a => a.StrategyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}