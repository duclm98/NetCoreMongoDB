using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NetCoreMongoDB.SubServices;

public interface IUserSubService
{
    string GenerateJsonWebToken(string id);
    string ValidateJsonWebToken(string token);
}

public class UserSubService : IUserSubService
{
    private readonly IConfiguration _configuration;

    public UserSubService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateJsonWebToken(string id)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:AccessTokenSecret"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
                new Claim("id", id)
            };

        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            null,
            DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpires"])),
            credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string ValidateJsonWebToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true, // có validate thời gian hết hạn
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey =
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:AccessTokenSecret"])),
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var id = jwtToken.Claims.First(x => x.Type == "id").Value;

            return id;
        }
        catch
        {
            return null;
        }
    }
}