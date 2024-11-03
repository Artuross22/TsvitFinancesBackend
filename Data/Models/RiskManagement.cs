using Data.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public class RiskManagement
{
    public int Id { get; set; }

    public Guid PublicId { get; set; }

    public required string Name { get; set; }

    public required RiskCategory Category { get; set; }

    public required decimal BaseRiskPercentage { get; set; }

    public required decimal RiskToRewardRatio { get; set; }

    public required Hedge Hedge {  get; set; }

    public required Diversification Diversification { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<RiskManagement>
    {
        public void Configure(EntityTypeBuilder<RiskManagement> builder)
        {
            builder.ToTable("RiskManagements");
        }
    }
}
