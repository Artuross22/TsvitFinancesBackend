using Microsoft.EntityFrameworkCore;

namespace Data.Db;

public class MainDb : DbContext
{
    public MainDb(DbContextOptions<MainDb> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
