using JWT.Models;

namespace JWT.Repository.Interfaces
{
    public interface IToken
    {
        string GetToken(User user, byte[] key);
    }
}
