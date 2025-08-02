using Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;

public class AssetHistory
{
    public int Id { get; set; }

    public required Guid PublicId { get; set; }

    public required decimal Quantity { get; set; }

    public required decimal Price { get; set; }

    public required PositionType Type { get; set; }

    public DateTime CreateAt { get; set; }

    public int AssetId { get; set; }
    public required Asset Asset { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<AssetHistory>
    {
        public void Configure(EntityTypeBuilder<AssetHistory> builder)
        {
            builder.ToTable("AssetHistory");

            builder.HasKey(e => e.Id);

            builder.HasOne(b => b.Asset)
                .WithMany()
                .HasForeignKey(b => b.AssetId);
        }
    }
}