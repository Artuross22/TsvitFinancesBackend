using Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;

public class BalanceFlow
{
    public int Id { get; set; }
    public required decimal Sum { get; set; }
    public required Balance Balance { get; set; }
    public required DateTime CreatedOn { get; set; }
    public required string? AppUserId { get; set; }
    public required AppUser? AppUser { get; set; }
    //public Balance? FromBalance { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<BalanceFlow>
    {
        public void Configure(EntityTypeBuilder<BalanceFlow> builder)
        {
            builder.ToTable("BalanceFlows");

            builder.HasKey(e => e.Id);

            builder.HasOne(b => b.AppUser)
                .WithMany(u => u.BalanceFlows)
                .HasForeignKey(b => b.AppUserId);
        }
    }
}
