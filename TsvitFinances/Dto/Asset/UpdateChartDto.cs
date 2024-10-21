namespace TsvitFinances.Dto.Asset;

public class UpdateChartDto
{
    public required Guid AssetId {  get; set; }

    public required int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }
}
