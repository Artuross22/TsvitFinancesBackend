namespace Data.Models;

public class SectorHedge
{
    public int Id { get; set; }

    public required Hedge Hadge { get; set; }
    public required int HedgeId { get; set; }
}
