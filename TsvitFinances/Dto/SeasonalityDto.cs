namespace TsvitFinances.Dto;

public class SeasonalityDto
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Description { get; set; }

    public DateTime StartSeason { get; set; }

    public DateTime EndSeason {  get; set; }
}
