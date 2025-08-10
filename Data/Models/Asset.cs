using Data.Exceptions;
using Data.Models.Enums;
using Data.Modelsl;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Models;

public class Asset
{
    public int Id { get; init; }

    public required Guid PublicId { get; set; }

    public required string ContractId { get; set; }

    public required string Goal { get; set; }

    public required Sector Sector { get; set; }

    public InvestmentTerm Term { get; set; }

    public required Market Market { get; set; }

    public required string Name { get; set; }

    public required string Ticker { get; set; }

    public required decimal CurrentPrice { get; set; }

    public required decimal Quantity { get; set; }

    public required decimal InterestOnCurrentDeposit { get; set; }

    public required decimal BoughtFor { get; set; }

    public required DateTime AddedAt { get; set; }

    public required bool IsActive { get; set; }

    public DateTime? ClosedAt { get; set; }

    public decimal? SoldFor { get; set; }

    public required string AppUserId { get; set; }
    public required AppUser AppUser { get; set; }

    public required int? StrategyId { get; set; }
    public virtual required Strategy? Strategy { get; set; }

    public required int? InvestmentIdeaId { get; set; }
    public virtual required InvestmentIdea? InvestmentIdea { get; set; }

    public int? SeasonalityId { get; set; }
    public Seasonality? Seasonalities { get; set; }

    public virtual required ICollection<PositionEntryNote> PositionEntryNotes { get; set; }

    public virtual required ICollection<SalesLevels> SalesLevels { get; set; }

    public virtual required ICollection<PurchaseLevel> PurchaseLevels { get; set; }

    private readonly List<AssetHistory> _assetHistories = [];

    public IReadOnlyCollection<AssetHistory> AssetHistories => _assetHistories.AsReadOnly();



    public decimal CurrentValue => CurrentQuantity * CurrentPrice;

    public decimal UnrealizedPnL => CurrentValue - CostBasis;

    public decimal UnrealizedPnLPercentage => CostBasis > 0 ? (UnrealizedPnL / CostBasis) * 100 : 0;

    public decimal AveragePrice => CurrentQuantity > 0 ? CostBasis / CurrentQuantity : 0;

    public decimal CostBasis
    {
        get
        {
            var totalBuyPositionsAmount = _assetHistories
                 .Where(ah => ah.Type == PositionType.Long)
                 .Sum(q => q.Quantity * q.Price);

            var totalShortPositionsAmount = _assetHistories
                .Where(ah => ah.Type == PositionType.Short)
                .Sum(q => q.Quantity * q.Price);

            return (BoughtFor * Quantity) + (totalBuyPositionsAmount - totalShortPositionsAmount);
        }
    }

    public decimal CurrentQuantity
    {
        get
        {
            var totalBuyPositionsAmount = _assetHistories
                 .Where(ah => ah.Type == PositionType.Long)
                 .Sum(q => q.Quantity);

            var totalShortPositionsAmount = _assetHistories
                .Where(ah => ah.Type == PositionType.Short)
                .Sum(q => q.Quantity);

            return Quantity + (totalBuyPositionsAmount - totalShortPositionsAmount);
        }
    }

    private void AddHistory(decimal quantity, decimal price, PositionType type)
    {
        var history = new AssetHistory(Guid.NewGuid(), quantity, price, type, Id);
        _assetHistories.Add(history);
    } 

    public void Buy(decimal quantity, decimal price)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be positive");
        if (!IsActive)
            throw new DomainException("Cannot buy inactive asset");

        AddHistory(quantity, price, PositionType.Long);
    }

    public void Sell(decimal quantity, decimal price)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity must be positive");
        if (quantity > CurrentQuantity)
            throw new DomainException("Cannot sell more than owned");
        if (!IsActive)
            throw new DomainException("Cannot sell inactive asset");
            
        AddHistory(quantity, price, PositionType.Short);
        
        if (CurrentQuantity <= 0)
        {
            Close(price);
        }   
    }

    public void Close(decimal soldFor)
    {
        if (!IsActive)
            throw new DomainException("Asset is already closed");

        IsActive = false;
        ClosedAt = DateTime.UtcNow;
        SoldFor = soldFor;
    } 

    internal class EFConfiguration : IEntityTypeConfiguration<Asset>
    {
        public void Configure(EntityTypeBuilder<Asset> builder)
        {
            builder.ToTable("Assets");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Ticker)
                .IsRequired()
                .HasMaxLength(10);

            builder.HasOne(a => a.AppUser)
                .WithMany(u => u.Assets)
                .HasForeignKey(a => a.AppUserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Strategy)
                .WithMany()
                .HasForeignKey(a => a.StrategyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.InvestmentIdea)
                .WithMany(u => u.Assets)
                .HasForeignKey(a => a.InvestmentIdeaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(s => s.SalesLevels)
                .WithOne(a => a.Asset)
                .HasForeignKey(a => a.AssetId);

            builder.HasMany(s => s.PurchaseLevels)
                .WithOne(a => a.Asset)
                .HasForeignKey(a => a.AssetId);

            builder.HasMany(s => s.PositionEntryNotes)
                .WithOne(a => a.Asset)
                .HasForeignKey(a => a.AssetId);

            builder.HasMany(a => a.AssetHistories)
                .WithOne(h => h.Asset)
                .HasForeignKey(h => h.AssetId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Ignore(a => a.CurrentValue);
            builder.Ignore(a => a.UnrealizedPnL);
            builder.Ignore(a => a.UnrealizedPnLPercentage);
            builder.Ignore(a => a.AveragePrice);
            builder.Ignore(a => a.CostBasis);
            builder.Ignore(a => a.CurrentQuantity);
        }
    }
}