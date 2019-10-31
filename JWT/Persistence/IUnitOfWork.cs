using JWT.Repository.Interfaces;

namespace JWT.Persistence
{
    public interface IUnitOfWork 
    {
        IUser User { get; }
        IToken Token { get; }
        void Complete();
    }
}
