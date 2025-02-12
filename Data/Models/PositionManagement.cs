using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;

public class PositionManagement
{
    public int Id { get; init; }

    public required Guid PublicId { get; init; }

    public required decimal? ScalingOut { get; set; }

    public required decimal? ScalingIn { get; set; }

    public required decimal AverageLevel { get; set; }

    public required ICollection<PositionScaling> PositionScalings { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<PositionManagement>
    {
        public void Configure(EntityTypeBuilder<PositionManagement> builder)
        {
            builder.ToTable("PositionEntries");
        }
    }
}