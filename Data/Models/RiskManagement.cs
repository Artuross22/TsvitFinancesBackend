using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;

public class RiskManagement
{
    public int Id { get; init; }

    public Guid PublicId { get; set; }

    public required string Name { get; set; }

    public required decimal BaseRiskPercentage { get; set; }

    public required decimal RiskToRewardRatio { get; set; }

    public required int? HedgeId { get; set; }
    public required virtual Hedge Hedge {  get; set; }

    public required virtual ICollection<Diversification> Diversification { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<RiskManagement>
    {
        public void Configure(EntityTypeBuilder<RiskManagement> builder)
        {
            builder.ToTable("RiskManagements");
        }
    }
}
