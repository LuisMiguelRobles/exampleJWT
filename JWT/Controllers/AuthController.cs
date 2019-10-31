using System.Net;
using System.Text;
using JWT.Models;
using JWT.Persistence;
using JWT.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

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
        public IActionResult Login(UserViewModel viewModel)
        {
            var user = _unitOfWork.User.GetUser(viewModel.Email);
            if (!ModelState.IsValid)
                return BadRequest(HttpStatusCode.NotFound);

            if (user == null)
                return NotFound();

            if (!BCrypt.Net.BCrypt.Verify(viewModel.Password, user.Password))
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