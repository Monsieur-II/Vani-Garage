using Microsoft.EntityFrameworkCore;
using Vani.Domain.Models;

namespace Vani.Infras;

public class VaniDbContext(DbContextOptions<VaniDbContext> options) : DbContext(options)
{
    public virtual DbSet<Car> Cars { get; set; }
    public virtual DbSet<Make> Makes { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Make>().HasData(
            new Make { Id = 1, Name = "Ford", Country = "USA" },
            new Make { Id = 2, Name = "Chevrolet", Country = "USA" },
            new Make { Id = 3, Name = "Toyota", Country = "Japan" },
            new Make { Id = 4, Name = "Nissan", Country = "Japan" },
            new Make { Id = 5, Name = "Honda", Country = "Japan" },
            new Make { Id = 6, Name = "BMW", Country = "Germany" }
        );
        modelBuilder.Entity<Car>()
            .HasOne(c => c.Make)
            .WithMany(m => m.Cars)
            .HasForeignKey(c => c.MakeId);
    }
}
