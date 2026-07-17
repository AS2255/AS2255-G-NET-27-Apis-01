using E_Commerce.Application.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace E_Commerce.Infrastructure.Identity.Services
{
    public class TokenServices : ITokenService
    {
        public async Task<string> CreateTokenAsync(string userId, string email, string userName, IReadOnlyList<string> roles)
        {
            // Header [type, algo]
            // Payloads [claims]
            // Signature [secret key]

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.GivenName, userName)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            //var secretKey = "MySeCretKeyFOrMyAppliCtionMySeCretKeyFOrMyAppliCtionMySeCretKeyFOrMyAppliCtionMySeCretKeyFOrMyAppliCtionMySeCretKeyFOrMyAppliCtion";
            
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySeCretKeyFOrMyAppliCtionMySeCretKeyFOrMyAppliCtionMySeCretKeyFOrMyAppliCtionMySeCretKeyFOrMyAppliCtionMySeCretKeyFOrMyAppliCtion"));


            var jwtToken = new JwtSecurityToken(
                issuer: "https://localhost:7081",
                audience: "My Online Store",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(2),
                signingCredentials: new SigningCredentials (SecurityKey, SecurityAlgorithms.HmacSha256Signature)

                );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
