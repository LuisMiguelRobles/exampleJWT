using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWT.Models;
using Microsoft.AspNetCore.Mvc;

namespace JWT.Repository.Interfaces
{
    public interface IUser
    {
        User GetUser(string email);

        void AddUser(User user);

    }
}
