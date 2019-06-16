using Common;
using FileProject.DAL;
using FileProject.Helpers;
using FileProject.Infra;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileProject.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly AppSettings _appSettings;


        public UserService(IUserRepository userRepo, IOptions<AppSettings> appSettings)
        {
            _userRepo = userRepo;
            _appSettings = appSettings.Value;
        }

        public async Task<User> Login(User user)
        {
            var myUser = await _userRepo.GetUser(user);
            if (user != null)
            {
                var password = HashMD5(user.Password);
                if (password == myUser.Password)
                    return myUser;
                else
                    throw new Exception("Wrong Password!");
            }
            throw new Exception("Wrong Username, or Password!");
        }

        public async Task<User> Register(User newUser)
        {
            var isThereUserinDB = await _userRepo.GetUser(newUser);
            if (isThereUserinDB == null)
            {
                newUser.Password = HashMD5(newUser.Password);
                var userFromDB = await _userRepo.RegisterUser(newUser);
                return userFromDB;
            }
            else
                throw new Exception("User Already Exists");
        }
        public string HashMD5(string password)
        {
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(password));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }


        private User GenerateToken(User user)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
               {
                    new Claim(ClaimTypes.Name, user.UserID.ToString())
               }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = null;
            return user;
        }
    }
}
