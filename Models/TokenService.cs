using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmollApi.Models
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        string GetClaim(string token, string claimType);
        bool ValidateCurrentToken(string token);
    }

    public class TokenService : ITokenService
    {
        private const int EXPIRY_DURATION_MINUTES = 1;
        private readonly IConfiguration configuration;

        public TokenService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var myIssuer = configuration.GetSection("Jwt:Issuer").ToString();
            var myAudience = configuration.GetSection("Jwt:Audience").ToString();

            var secretKey = configuration.GetSection("Jwt:Key").ToString();
            
            var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("UserRole", user.AccounType),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(EXPIRY_DURATION_MINUTES)),
                Issuer = myIssuer,
                Audience = myAudience,
                SigningCredentials = credentials
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Authorisation method.
        /// This method checks the token and returns true or false.
        /// </summary>
        /// <param name="token">the jwt token of the use</param>
        /// <returns>boolean</returns>
        public bool ValidateCurrentToken(string token)
        {
            var secretKey = configuration.GetSection("Jwt:Key").ToString();
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            var myIssuer = configuration.GetSection("Jwt:Issuer").ToString();
            var myAudience = configuration.GetSection("Jwt:Audience").ToString();

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = myIssuer,
                    ValidAudience = myAudience,
                    IssuerSigningKey = mySecurityKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public string GetClaim(string token, string claimType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

            var stringClaimValue = securityToken.Claims
                .FirstOrDefault(claim => claim.Type == claimType).Value;

            return stringClaimValue;
        }
        
    }

}
