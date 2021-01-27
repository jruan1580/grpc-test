using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using User.Infrastructure.Repository.Entities;

namespace User.Infrastructure.Repository
{
    public interface IUserRepository
    {
        Task<List<Entities.User>> GetUsers();
        Task<Entities.User> GetUserById(Guid userId);
        Task<Entities.User> GetUserByEmail(string email);
        Task CreateUser(string name, string email, byte[] password);
        Task UpdateStatus(string email, bool status);
    }

    public class UsersRepository : IUserRepository
    {
        private readonly string _dbPath;

        public UsersRepository()
        {
            var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var path = Path.Combine(outPutDirectory, "Repository\\Database\\Users.txt");

            if (!File.Exists(path))
            {
                throw new Exception("Unable to locate user db");
            }

            _dbPath = path;
        }

        public async Task CreateUser(string name, string email, byte[] password)
        {
            var newUser = new Entities.User()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                Password = password,
                LoggedOn = false
            };

            var userJson = await File.ReadAllTextAsync(_dbPath);

            var users = (string.IsNullOrEmpty(userJson)) 
                ? new Users() { Accounts = new List<Entities.User>() } 
                : JsonConvert.DeserializeObject<Users>(userJson);
         

            users.Accounts.Add(newUser);

            await File.WriteAllTextAsync(_dbPath, JsonConvert.SerializeObject(users));
        }

        public async Task<Entities.User> GetUserByEmail(string email)
        {
            var userJson = await File.ReadAllTextAsync(_dbPath);

            if (string.IsNullOrEmpty(userJson))
            {
                return null;
            }

            var users = JsonConvert.DeserializeObject<Users>(userJson);

            return users.Accounts.FirstOrDefault(u => u.Email.Equals(email));
        }

        public async Task<Entities.User> GetUserById(Guid userId)
        {
            var userJson = await File.ReadAllTextAsync(_dbPath);

            if (string.IsNullOrEmpty(userJson))
            {
                return null;
            }

            var users = JsonConvert.DeserializeObject<Users>(userJson);

            return users.Accounts.FirstOrDefault(u => u.Id == userId);
        }

        public async Task<List<Entities.User>> GetUsers()
        {
            var userJson = await File.ReadAllTextAsync(_dbPath);

            if (string.IsNullOrEmpty(userJson))
            {
                return new List<Entities.User>();
            }

            var users = JsonConvert.DeserializeObject<Users>(userJson);

            return users.Accounts;
        }

        public async Task UpdateStatus(string email, bool status)
        {
            var userJson = await File.ReadAllTextAsync(_dbPath);

            if (string.IsNullOrEmpty(userJson))
            {
                return;
            }

            var users = JsonConvert.DeserializeObject<Users>(userJson);

            foreach(var user in users.Accounts)
            {
                if (!user.Email.Equals(email))
                {
                    continue;
                }

                user.LoggedOn = status;
            }

            await File.WriteAllTextAsync(_dbPath, JsonConvert.SerializeObject(users));
        }
    }
}
