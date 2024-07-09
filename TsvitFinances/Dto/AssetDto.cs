using Data.Models;

namespace TsvitFinances.Dto
{
    public class AssetDto
    {
        public int Id { get; init; }
        public Guid PublicId { get; set; }

        public required string Name { get; set; }

        public decimal CurrentPrice { get; set; }

        public decimal BoughtFor { get; set; }

        public DateTime AddedAt { get; set; }

        public bool IsActive { get; set; }

        public DateTime? ClosedAt { get; set; }

        public decimal? SoldFor { get; set; }

        public virtual IEnumerable<Chart> Charts { get; set; } = [];
    }
}
