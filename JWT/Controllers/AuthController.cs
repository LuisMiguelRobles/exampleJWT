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
using JWT.Persistence;
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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string email, string password)
        {
            var user = _unitOfWork.User.GetUser(email);
            if (!ModelState.IsValid)
                return BadRequest(HttpStatusCode.NotFound);

            if (user == null)
                return NotFound();

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return NotFound();
            }
           

            return Ok(_unitOfWork.Token.GetToken(user,GetKey()));
        }

        [HttpPost]
        [Route("~/api/save")]
        //public IActionResult AddUser(string email, string password)
        public IActionResult AddUser(User request)
        {
            if (request == null)
                return BadRequest();

            var user = new User
            {
                Name = request.Name ?? string.Empty,
                LastName = request.LastName ?? string.Empty,
                UserName = request.UserName ?? string.Empty,
                Email = request.Email ?? string.Empty,
                Password = BCrypt.Net.BCrypt.HashPassword(request.Password) ?? string.Empty
            };
            _unitOfWork.User.AddUser(user);
            _unitOfWork.Complete();
            return Ok(user);
        }


        private byte[] GetKey()
        {
            var secretKey = _configuration.GetValue<string>("JWT:secretKey");
            var key = Encoding.ASCII.GetBytes(secretKey);
            return key;
        }
    }
}