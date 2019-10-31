using JWT.Models;

namespace JWT.Repository.Interfaces
{
    public interface IUser
    {
        User GetUser(string email);

        void AddUser(User user);

    }
}
