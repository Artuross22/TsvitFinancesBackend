namespace TsvitFinances.Dto.Asset;

public class ChartDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public IFormFile File { get; set; }
}
