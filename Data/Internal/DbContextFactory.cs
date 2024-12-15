using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Data.Internal;

public class DbContextFactory : IDesignTimeDbContextFactory<MainDb>
{
    public MainDb CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.json")
         .Build();

        var optionsBuilder = new DbContextOptionsBuilder<MainDb>();
        return(new MainDb(optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection")).Options));
    }
}