using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Vani.Domain.Models;

namespace Vani.Infras;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<IdentityRole>()
            .HasData(
                new IdentityRole { Name = VaniRoles.Admin_User, NormalizedName = VaniRoles.Admin_User.ToUpper() },
                new IdentityRole { Name = VaniRoles.Staff_User, NormalizedName = VaniRoles.Staff_User.ToUpper() },
                new IdentityRole { Name = VaniRoles.User_User, NormalizedName = VaniRoles.User_User.ToUpper() },
                new IdentityRole { Name = VaniRoles.Agent_User, NormalizedName = VaniRoles.Agent_User.ToUpper() },
                new IdentityRole { Name = VaniRoles.Developer_User, NormalizedName = VaniRoles.Developer_User.ToUpper() }
            );

    }
}
