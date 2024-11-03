using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public class Hedge
{
    public int Id { get; set; }

    public required string Name { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<Hedge>
    {
        public void Configure(EntityTypeBuilder<Hedge> builder)
        {
            builder.ToTable("Hedges");
        }
    }
}
