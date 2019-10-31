using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JWT.Connection;
using JWT.Models;
using JWT.Repository.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace JWT.Repository
{
    public class Token : IToken
    {
        
        private readonly JwtContext _context;

        public Token(JwtContext context)
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
