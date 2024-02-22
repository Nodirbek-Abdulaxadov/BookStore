using BookStore.IdentityService.Data.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookStore.IdentityService.Extend;


public static class JwtHelper
{
    public static string GenerateJwtToken(User user,
                                          IList<string> roles,
                                          IConfiguration configuration)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
        var issuer = configuration["Jwt:Issuer"];
        var audience = configuration["Jwt:Audience"];

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
             {
                new Claim(ClaimTypes.Name, user.Email!),
                new Claim(ClaimTypes.GivenName, $"{user.FullName}"),
                new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
             }),
            Expires = DateTime.UtcNow.AddMonths(1),
            Audience = audience,
            Issuer = issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        foreach (var role in roles)
        {
            tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
        }

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}