using Microsoft.EntityFrameworkCore;

namespace Data.Db;

public class MainDb : DbContext
{
    public MainDb(DbContextOptions<MainDb> options) : base(options)
    {

    }
}
