using Data.Models;
using Data.Modelsl;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

public class StrategyMacroeconomicEvent
{
    public required int StrategyId { get; set; }
    public required Strategy Strategy { get; set; }

    public required int MacroeconomicEventId { get; set; }
    public required MacroeconomicEvent MacroeconomicEvent { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<StrategyMacroeconomicEvent>
    {
        public void Configure(EntityTypeBuilder<StrategyMacroeconomicEvent> builder)
        {
            builder.ToTable("StrategyMacroeconomicEvents");

            builder.HasKey(x => new { x.StrategyId, x.MacroeconomicEventId });

            builder.HasOne(x => x.Strategy)
                   .WithMany(s => s.StrategyMacroeconomicEvents)
                   .HasForeignKey(x => x.StrategyId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.MacroeconomicEvent)
                   .WithMany(e => e.StrategyMacroeconomicEvents)
                   .HasForeignKey(x => x.MacroeconomicEventId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
