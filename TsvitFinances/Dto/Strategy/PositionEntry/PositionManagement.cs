namespace TsvitFinances.Dto.Strategy.PositionEntry;

public class PositionManagement
{
    public required Guid PublicId { get; init; }

    public required decimal? ScalingOut { get; set; }

    public required decimal? ScalingIn { get; set; }

    public required double AverageLevel { get; set; }
}
