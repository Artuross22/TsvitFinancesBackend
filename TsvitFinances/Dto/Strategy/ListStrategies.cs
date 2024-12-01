namespace TsvitFinances.Dto.Strategy;

public class ListStrategies
{
    public required Guid PublicId { get; set; }
    public required string Name {  get; set; } 
    public required bool IsSetToCurrentAsset { get; set; }
}
