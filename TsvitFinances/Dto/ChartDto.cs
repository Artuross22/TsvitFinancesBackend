namespace TsvitFinances.Dto;

public class ChartDto
{
    public int Id { get; set; }

    public required int AssetId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public required byte[] ImageData { get; set; }
}
