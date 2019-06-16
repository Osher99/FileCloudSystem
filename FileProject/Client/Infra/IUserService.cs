using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace Client.Infra
{
    public interface IUserService
    {

       Task<User> RegisterAsync(User user);

       Task<User> SignInAsync(User user);
       IEnumerable<User> GetAllUsers();

    }
}
