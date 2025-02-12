using Data.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public class PositionScaling
{
    public int Id { get; init; }
    public required Guid PublicId { get; init; }
    public required decimal EquityPercentage { get; set; }
    public required PositionType PositionType { get; set; }
    public required int PositionManagementId { get; set; }
    public required PositionManagement PositionManagement { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<PositionScaling>
    {
        public void Configure(EntityTypeBuilder<PositionScaling> builder)
        {
            builder.ToTable("PositionScalings");

            builder.HasOne(s => s.PositionManagement)
                .WithMany(a => a.PositionScalings)
                .HasForeignKey(a => a.PositionManagementId);
        }
    }
}