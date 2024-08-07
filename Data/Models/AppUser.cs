using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;

public class AppUser : IdentityUser
{
    public string Nickname { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName {get ; set; } = string.Empty;
    public DateTime CreatedOn { get; set; }
    public virtual IEnumerable<BalanceFlow>? BalanceFlows { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers");
        }
    }
}
