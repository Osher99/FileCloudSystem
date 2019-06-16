using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileProject.Infra
{
    public interface IUserRepository
    {
        Task<User> GetUser(User user);

        Task<User> RegisterUser(User newUser);

        Task<User> LoginUser(User loggedUser);
    }
}
