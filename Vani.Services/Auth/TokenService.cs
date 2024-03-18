using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Vani.Domain.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Vani.Services.Auth;

public static class TokenService
{
    public static async Task<string> GenerateTokenAsync(AppUser user, UserManager<AppUser> userManager,
        JWTConfig config)
    {
        var userClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.NameIdentifier, user.Id)
        };
        var userRoles = await userManager.GetRolesAsync(user);
        foreach (var userRole in userRoles)
        {
            userClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.TokenKey));
        var token = new JwtSecurityToken(
            issuer: config.ValidIssuer,
            audience: config.ValidAudience,
            expires: DateTime.Now.AddHours(config.TokenExpiry),
            claims: userClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
