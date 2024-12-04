using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;

public class SalesLevels
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public required decimal Level { get; set; }
    public required decimal? AverageLevel { get; set; }
    public required int PositionManagementId { get; set; }
    public required PositionManagement PositionManagement { get; set; }
    internal class EFConfiguration : IEntityTypeConfiguration<SalesLevels>
    {
        public void Configure(EntityTypeBuilder<SalesLevels> builder)
        {
            builder.ToTable("SalesLevels");
        }
    }
}
