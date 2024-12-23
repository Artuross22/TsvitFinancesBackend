using Data.Modelsl;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;

public class AppUser : IdentityUser
{
    public Guid PublicId { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public virtual ICollection<Asset>? Assets { get; set; }
    public virtual ICollection<Strategy>? Strategies { get; set; }
    public virtual IEnumerable<BalanceFlow>? BalanceFlows { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers");

            builder.HasKey(e => e.Id);

            builder.HasMany(u => u.Assets)
                    .WithOne(a => a.AppUser)
                    .HasForeignKey(a => a.AppUserId);

            builder.HasMany(u => u.Strategies)
                    .WithOne(a => a.AppUser)
                    .HasForeignKey(a => a.AppUserId);

            builder.HasMany(u => u.BalanceFlows)
                   .WithOne(a => a.AppUser)
                   .HasForeignKey(a => a.AppUserId);
        }
    }
}
