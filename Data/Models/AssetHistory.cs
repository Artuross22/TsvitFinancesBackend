using Data.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;

public class AssetHistory
{
    public int Id { get; private set; }
    public Guid PublicId { get; private set; }
    public decimal Quantity { get; private set; }
    public decimal Price { get; private set; }
    public PositionType Type { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public int AssetId { get; private set; }
    public Asset Asset { get; private set; } = null!;

    private AssetHistory() { }

    internal AssetHistory(Guid publicId, decimal quantity, decimal price, PositionType type,int assetId)
    {
        ValidateInputs(quantity, price);
        
        PublicId = publicId;
        Quantity = quantity;
        Price = price;
        Type = type;
        CreatedAt = DateTime.UtcNow;
        AssetId = assetId;
    }

    private static void ValidateInputs(decimal quantity, decimal price)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be greater than zero", nameof(quantity));
        
        if (price <= 0)
            throw new ArgumentException("Price must be greater than zero", nameof(price));
    }

    public decimal GetTotalValue() => Quantity * Price;
    
    public bool IsLongPosition() => Type == PositionType.Long;
    
    public bool IsShortPosition() => Type == PositionType.Short;

    internal class EFConfiguration : IEntityTypeConfiguration<AssetHistory>
    {
        public void Configure(EntityTypeBuilder<AssetHistory> builder)
        {
            builder.ToTable("AssetHistory");

            builder.HasKey(e => e.Id);
        }
    }
}