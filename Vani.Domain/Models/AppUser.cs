using Microsoft.AspNetCore.Identity;

namespace Vani.Domain.Models;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime LastLogin { get; set; }
    public string FullName => $"{FirstName} {LastName}";
    public DateTime TokenExpiry { get; set; }
}
