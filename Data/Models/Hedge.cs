using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;

public class Hedge
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public virtual ICollection<Option>? Options { get; set; }

    public virtual ICollection<Futures>? Futures { get; set; }

    public virtual ICollection<SectorHedge>? SectorHedges { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<Hedge>
    {
        public void Configure(EntityTypeBuilder<Hedge> builder)
        {
            builder.ToTable("Hedges");

            builder.HasMany(s => s.Options)
                .WithOne(a => a.Hadge)
                .HasForeignKey(a => a.HedgeId);

            builder.HasMany(s => s.Futures)
                .WithOne(a => a.Hadge)
                .HasForeignKey(a => a.HedgeId);

            builder.HasMany(s => s.SectorHedges)
                .WithOne(a => a.Hadge)
                .HasForeignKey(a => a.HedgeId);
        }
    }
}
