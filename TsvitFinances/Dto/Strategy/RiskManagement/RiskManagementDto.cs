using Data.Models.Enums;

namespace TsvitFinances.Dto.Strategy.RiskManagement;

public class RiskManagementDto
{
    public required Guid PublicId { get; set; }
    public required decimal RiskToRewardRatio { get; set; }
    public required decimal BaseRiskPercentage { get; set; }
    public required int Category { get; set; }
}
