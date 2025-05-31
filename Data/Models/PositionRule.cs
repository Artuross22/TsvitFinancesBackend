using Data.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public class PositionRule
{
    public int Id { get; init; }

    public required Guid PublicId { get; init; }

    public required int MinimumCorrectionPercent { get; set; }

    public required TimeFrame TimeFrame { get; set; }

    public required int PositionManagementId { get; set; }
    public required PositionManagement PositionManagement { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<PositionRule>
    {
        public void Configure(EntityTypeBuilder<PositionRule> builder)
        {
            builder.ToTable("PositionRules");
        }
    }
}