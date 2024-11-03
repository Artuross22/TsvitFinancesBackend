namespace TsvitFinances.Dto.Asset;

public class AddChartDto
{
    public Guid AssetId { get; set; }

    public List<ChartDto> Charts { get; set; } = new List<ChartDto>();
}