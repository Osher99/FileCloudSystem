using Common;
using FileProject.Data;
using FileProject.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileProject.DAL
{
    public class UserRepository: IUserRepository
    {
        public readonly UserContext _userContext;
        public UserRepository(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task<User> GetUser(User user)
        {
            var foundUser = _userContext.Users.FirstOrDefault(u => u.Email == user.Email);
            return foundUser;
        }

        public async Task<User> RegisterUser(User newUser)
        {
            try
            {
                _userContext.Users.Add(newUser);
                await _userContext.SaveChangesAsync();
                var registeredUser = await GetUser(newUser);
                return registeredUser;
            }

            catch (Exception)
            {
                return null;
            }
        }

        public async Task<User> LoginUser(User loggedUser)
        {
            return await GetUser(loggedUser);
        }
    }
}
