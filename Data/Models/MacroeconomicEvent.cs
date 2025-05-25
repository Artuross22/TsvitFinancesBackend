using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Data.Modelsl;

namespace Data.Models;

public class MacroeconomicEvent
{
    public int Id { get; set; }

    public required Guid PublicId { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }

    public required int Rating { get; set; }
     
    public required DateTime CreateAt { get; set; }

    public required string Source { get; set; }

    public required int MacroeconomicAnalysisId { get; set; }

    public required MacroeconomicAnalysis MacroeconomicAnalyses { get; set; }

    public ICollection<Strategy>? Strategies { get; set; }

    public virtual ICollection<StrategyMacroeconomicEvent>? StrategyMacroeconomicEvents { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<MacroeconomicEvent>
    {
        public void Configure(EntityTypeBuilder<MacroeconomicEvent> builder)
        {
            builder.ToTable("MacroeconomicEvents");
        }
    }
}