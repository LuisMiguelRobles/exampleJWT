using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JWT.Models;
using JWT.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWT.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = UserList.SingleOrDefault(x => x.Email == email && x.Password == password);
            if (user == null)
                return BadRequest(HttpStatusCode.NotFound);

            var secretKey = _configuration.GetValue<string>("JWT:secretKey");
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var createToken = tokenHandler.CreateToken(GetSecurityTokenDescriptor(key, user));


            return Ok(tokenHandler.WriteToken(createToken));
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

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            return tokenDescriptor;
        }



        private IEnumerable<User> UserList = new List<User>()
        {
            new User{Email ="prueba1@gmail.com",Id= 1, Password= "prueba", UserName= "prueba1"},
            new User{Email ="prueba2@gmail.com",Id= 2, Password= "prueba", UserName= "prueba2"},
            new User{Email ="prueba3@gmail.com",Id= 3, Password= "prueba", UserName= "prueba3"},
            new User{Email ="prueba4@gmail.com",Id= 4, Password= "prueba", UserName= "prueba4"},
            new User{Email ="prueba5@gmail.com",Id= 5, Password= "prueba", UserName= "prueba5"}
        };
    }
}