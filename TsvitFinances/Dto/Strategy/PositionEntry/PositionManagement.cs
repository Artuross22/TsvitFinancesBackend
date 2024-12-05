using Data.Models.Enums;

namespace TsvitFinances.Dto.Strategy.PositionEntry;

public class PositionManagementDto
{
    public required Guid PublicId { get; init; }

    public required decimal? ScalingOut { get; set; }

    public required decimal? ScalingIn { get; set; }

    public required decimal AverageLevel { get; set; }
}
