using JWT.Connection;
using JWT.Repository;
using JWT.Repository.Interfaces;

namespace JWT.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly JwtContext _context;
        public IUser User { get; }

        public IToken Token { get; }

        public UnitOfWork(JwtContext context)
        {
            _context = context;
            User = new UserRepository(_context);
            Token = new Token(_context);

        }
        
        public void Complete()
        {
            _context.SaveChanges();
        }
    }
}
