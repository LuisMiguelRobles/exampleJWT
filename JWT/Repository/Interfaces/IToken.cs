using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWT.Models;
using Microsoft.IdentityModel.Tokens;

namespace JWT.Repository.Interfaces
{
    public interface IToken
    {
        string GetToken(User user, byte[] key);
    }
}
