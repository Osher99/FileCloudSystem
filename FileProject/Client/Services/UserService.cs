using Client.Infra;
using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{

    public class UserService : IUserService
    {
        private Uri BaseUri { get; set; }
        public UserService()
        {
            BaseUri = new Uri("https://localhost:44339/api/user");
        }

        public async Task<User> RegisterAsync(User user)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{BaseUri}/register", content);
                var result = await response.Content.ReadAsStringAsync();
                var UserFromDB = JsonConvert.DeserializeObject<User>(result);
                if (response.IsSuccessStatusCode == true)
                {
                    return UserFromDB;
                }
                return null;
            }
        }

        public async Task<User> SignInAsync(User user)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

                var response = client.PostAsync($"{BaseUri}/login", content).Result;
                var result = await response.Content.ReadAsStringAsync();
                var UserFromDB = JsonConvert.DeserializeObject<User>(result);
                if (response.IsSuccessStatusCode == true)
                {
                    return UserFromDB;
                }
                return null;
            }
        }
        public IEnumerable<User> GetAllUsers()
        {
            Uri BaseUri = new Uri("http://localhost:52527/api/user");
            IEnumerable<User> users;
            using (var client = new HttpClient())
            {
                var OnlineUsers = client.GetAsync($"{BaseUri}/GetOnlineUsers").Result.Content.ReadAsStringAsync().Result;
                users = JsonConvert.DeserializeObject<IEnumerable<User>>(OnlineUsers);
            }
            return users;
        }

    }
}
