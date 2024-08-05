using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;

public class Seasonality
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public DateTime StartSeason { get; set; }

    public DateTime EndSeason { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<Seasonality>
    {
        public void Configure(EntityTypeBuilder<Seasonality> builder)
        {
            builder.ToTable("Seasonalities");
        }
    }
}
