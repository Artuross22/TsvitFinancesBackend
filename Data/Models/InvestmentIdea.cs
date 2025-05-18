using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public class InvestmentIdea
{
    public int Id { get; set; }

    public required Guid PublicId { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public required decimal ExpectedReturn { get; set; }

    public required decimal? Profit { get; set; }

    public required DateTime CreatedAt { get; set; }

    public required DateTime? ClosedAt { get; set; }

    public required string AppUserId { get; set; }

    public required AppUser AppUser { get; set; }

    public virtual required ICollection<Asset>? Assets { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<InvestmentIdea>
    {
        public void Configure(EntityTypeBuilder<InvestmentIdea> builder)
        {
            builder.ToTable("InvestmentIdeas");
        }
    }
}