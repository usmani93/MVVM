using ChatAPI.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatAPI.Helper
{
    public class AuthenticateUser : IAuthenticateUser
    {
        private string key;

        public AuthenticateUser(string key)
        {
            this.key = key;
        }

        public string AuthenticateUserWithJWT(string userName)
        {
            if(string.IsNullOrEmpty(userName))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userName)
                }),
                Expires = DateTime.UtcNow.AddMonths(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature),
                Audience = Utils.ValidAudience,
                Issuer = Utils.ValidAudience
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool TokenValidator(string token)
        {
            try
            {
                var jwtTokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(Utils.key);
                jwtTokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidAudience = Utils.ValidAudience,
                    ValidIssuer = Utils.ValidIssuer,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    ValidateAudience = true,
                    ValidateIssuer = true
                }, out SecurityToken securityToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
