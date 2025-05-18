using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Data.Models.Enums;

namespace Data.Models;

public class MacroeconomicAnalysis
{
    public int Id { get; set; }

    public required Guid PublicId { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public required EconomicType EconomicType { get; set; }

    public required string Source { get; set; }

    public required string AppUserId { get; set; }

    public required AppUser AppUser { get; set; }

    public virtual ICollection<MacroeconomicEvent>? MacroeconomicEvents { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<MacroeconomicAnalysis>
    {
        public void Configure(EntityTypeBuilder<MacroeconomicAnalysis> builder)
        {
            builder.ToTable("MacroeconomicAnalyses");
        }
    }
}