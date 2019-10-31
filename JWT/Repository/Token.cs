using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JWT.Connection;
using JWT.Models;
using JWT.Repository.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWT.Repository
{
    public class Token : IToken
    {
        
        private readonly JWTContext _context;

        public Token(JWTContext context)
        {
            _context = context;
        }

        public string GetToken(User user,byte[] key)
        {
            if (user == null) return string.Empty;
            var tokenHandler = new JwtSecurityTokenHandler();
            var createToken = tokenHandler.CreateToken(GetSecurityTokenDescriptor(key, user));
            var token = tokenHandler.WriteToken(createToken);
            return token;
        }

        
        private ClaimsIdentity GetUserClaims(User user)
        {
            var claims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Email, user.Email)
            });

            return claims;
        }

        private SecurityTokenDescriptor GetSecurityTokenDescriptor(byte[] key, User user)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GetUserClaims(user),

                Expires = DateTime.UtcNow.AddMinutes(2),

                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            return tokenDescriptor;
        }
    }
}
