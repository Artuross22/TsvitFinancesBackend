namespace TsvitFinances.Dto.Asset.Output;

public class GetCharts
{
    public required Guid AssetPublicId { get; set; }
    public IList<_Chart>? Charts { get; set; } = [];

    public class _Chart
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ChartsPath { get; set; } = string.Empty;
    }
}