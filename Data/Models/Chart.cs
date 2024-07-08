namespace Data.Models;

public class Chart
{
    public int Id { get; set; }

    public required int AssetId { get; set; } 

    public required Asset Asset { get; set; }    

    public string? Title { get; set; }

    public string? Description { get; set; }

    public required byte[] ImageData { get; set; }
}


