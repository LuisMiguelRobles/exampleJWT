using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWT.Models;
using Microsoft.EntityFrameworkCore;

namespace JWT.Connection
{
    public class JWTContext :DbContext
    {
        public JWTContext(DbContextOptions<JWTContext> options)
            :base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
    }
}
