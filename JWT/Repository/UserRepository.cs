using System.Linq;
using JWT.Connection;
using JWT.Models;
using JWT.Repository.Interfaces;

namespace JWT.Repository
{
    public class UserRepository : IUser
    {
        private readonly JWTContext _context;

        public UserRepository(JWTContext context)
        {
            _context = context;
        }

        public User GetUser(string email)
        {
            return _context.Users.SingleOrDefault(x=> x.Email.Equals(email));
        }

        public void AddUser(User user)
        {
            _context.Users.Add(user);
        }
    }
}