using Data.Models.Enums;

namespace TsvitFinances.Dto.AssetDto
{
    public class AssetUpdateDto
    {
        public int Id { get; init; }

        public Guid PublicId { get; set; }

        public required string UserPublicId { get; set; }

        public required Sector Sector { get; set; }

        public required string Name { get; set; }

        public required string Ticker { get; set; }

        public decimal CurrentPrice { get; set; }

        public decimal BoughtFor { get; set; }

        public int? SeasonalityId { get; set; }
        public SeasonalityDto? Seasonalities { get; set; }

        public virtual IEnumerable<ChartDto> Charts { get; set; } = [];
    }
}
