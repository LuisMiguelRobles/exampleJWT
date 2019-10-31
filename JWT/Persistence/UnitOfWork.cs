using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWT.Connection;
using JWT.Repository;
using JWT.Repository.Interfaces;

namespace JWT.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly JWTContext _context;
        public IUser User { get; }

        public IToken Token { get; }

        public UnitOfWork(JWTContext context)
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
