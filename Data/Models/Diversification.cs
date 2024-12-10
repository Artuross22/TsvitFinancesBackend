using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Data.Models.Enums;

namespace Data.Models;

public class Diversification
{
    public int Id { get; set; }

    public decimal NichePercentage { get; set; }

    public required Sector Sector { get; set; }

    public required int RiskManagementId { get; set; }

    public required RiskManagement RiskManagement { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<Diversification>
    {
        public void Configure(EntityTypeBuilder<Diversification> builder)
        {
            builder.ToTable("Diversifications");

            builder.HasOne(a => a.RiskManagement)
                .WithMany(u => u.Diversification)
                .HasForeignKey(a => a.RiskManagementId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
