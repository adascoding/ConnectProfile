using ConnectProfile.Api.Entities;
using ConnectProfile.Api.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ConnectProfile.Api.Services;

public class JwtService(IConfiguration configuration) : IJwtService
{
    public string GenerateToken(Account account)
    {
        var issuer = configuration["Jwt:Issuer"];
        var audience = configuration["Jwt:Audience"];
        var key = configuration["Jwt:Key"];
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        var claims = new[]
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, account.UserName),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, account.Role),
            new System.Security.Claims.Claim("Id", account.Id.ToString())
        };

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
