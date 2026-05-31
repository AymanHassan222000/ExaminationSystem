using ExaminationSystem.DAL.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ExaminationSystem.Helpers.Auth;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSettings _jwt;
    public JwtTokenGenerator(IOptions<JwtSettings> jwt)
    {
        _jwt = jwt.Value;
    }

    public string GenerateToken(int userID, string email, int roleID) 
    {
        var key = Encoding.ASCII.GetBytes(_jwt.Key);
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = _jwt.Issure,
            Audience = _jwt.Audience,

            Expires = DateTime.UtcNow.AddMinutes(_jwt.Lifetime),
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier,userID.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, $"{(UserRoles)roleID}")
            }),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            ),
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
