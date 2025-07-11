using Microsoft.EntityFrameworkCore;

namespace Model
{
  public class ApplicationDbContext : DbContext
  {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
      : base(options)
    {
    }

    // Add your DbSet properties here
    // public DbSet<YourEntity> YourEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Configure your entity relationships and constraints here
    }
    
    public DbSet<Model.Entity.User> Users { get; set; }
  }
}