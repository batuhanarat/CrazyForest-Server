using Microsoft.EntityFrameworkCore;
using SharedLibrary;

namespace Server;

public class GameDbContext : DbContext 
{
    public GameDbContext(DbContextOptions<GameDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Hero> Heroes { get; set; }
  

}