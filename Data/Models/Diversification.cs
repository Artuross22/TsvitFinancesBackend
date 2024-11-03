using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Data.Models;

public class Diversification
{ 
    public int Id { get; set; }
    public required string Name { get; set; }

    internal class EFConfiguration : IEntityTypeConfiguration<Diversification>
    {
        public void Configure(EntityTypeBuilder<Diversification> builder)
        {
            builder.ToTable("Diversifications");
        }
    }
}
