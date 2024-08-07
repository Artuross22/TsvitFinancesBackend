using Data.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public class BalanceFlow
{
    public int Id { get; set; }
    public required decimal Sum { get; set; }
    public required Balance Balance { get; set; }
    public required DateTime CreatedOn { get; set; }
    public required int AppUserId {  get; set; } 
    public required AppUser AppUser { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<BalanceFlow>
    {
        public void Configure(EntityTypeBuilder<BalanceFlow> builder)
        {
            builder.ToTable("BalanceFlows");
        }
    }
}
