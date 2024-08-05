using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;

public class Chart
{
    public int Id { get; set; }

    public required int AssetId { get; set; } 

    public required Asset Asset { get; set; }    

    public string? Title { get; set; }

    public string? Description { get; set; }

    public required byte[] ImageData { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<Chart>
    {
        public void Configure(EntityTypeBuilder<Chart> builder)
        {
            builder.ToTable("Charts");
        }
    }
}