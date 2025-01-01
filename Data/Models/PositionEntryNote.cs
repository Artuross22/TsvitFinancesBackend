using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public class PositionEntryNote
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public DateTime CreateAt { get; set; }

    public string? Note { get; set; }

    public required int AssetId { get; set; }

    public required Asset Asset { get; set; }

    public IEnumerable<Chart>? Charts { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<PositionEntryNote>
    {
        public void Configure(EntityTypeBuilder<PositionEntryNote> builder)
        {
            builder.ToTable("PositionEntryNotes");

            builder.HasMany(s => s.Charts)
                .WithOne(a => a.PositionEntryNote)
                .HasForeignKey(a => a.PositionEntryNoteId);
        }
    }
}