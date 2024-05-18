using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Db;

public class MainDb : DbContext
{
    public MainDb(DbContextOptions<MainDb> options) : base(options)
    {

    }

    public DbSet<Asset> Assets { get; set; }
}
