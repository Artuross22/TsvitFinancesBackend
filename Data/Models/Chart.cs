using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;

public class Chart
{
    public int Id { get; set; }

    public required int? PositionEntryNoteId { get; set; }

    public required PositionEntryNote? PositionEntryNote { get; set; }

    public required string FileName { get; set; }

    public required string FilePath { get; set; }

    public required long FileSize { get; set; }

    public DateTime UploadedDate { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<Chart>
    {
        public void Configure(EntityTypeBuilder<Chart> builder)
        {
            builder.ToTable("Charts");

            builder.HasOne(chart => chart.PositionEntryNote)
               .WithMany()
               .HasForeignKey(chart => chart.PositionEntryNoteId)
               .OnDelete(DeleteBehavior.Cascade); 
        }
    }
}