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
        Task<Entities.User> GetUserByEmail(string email);
        Task CreateUser(string name, string email, string password);
    }

    public class UsersRepository : IUserRepository
    {
        private readonly string _dbPath;

        public UsersRepository()
        {
            var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

            var path = Path.Combine(outPutDirectory, "Repository\\Database\\Users.txt");

            if (File.Exists(path))
            {
                throw new Exception("Unable to locate user db");
            }

            _dbPath = path;
        }

        public async Task CreateUser(string name, string email, string password)
        {
            var newUser = new Entities.User()
            {
                Name = name,
                Email = email,
                Password = password,
                LoggedOn = false
            };

            var userJson = await File.ReadAllTextAsync(_dbPath);

            var users = JsonConvert.DeserializeObject<Users>(userJson);

            users.Accounts.Add(newUser);

            await File.WriteAllTextAsync(_dbPath, JsonConvert.SerializeObject(users));
        }

        public async Task<Entities.User> GetUserByEmail(string email)
        {
            var userJson = await File.ReadAllTextAsync(_dbPath);

            var users = JsonConvert.DeserializeObject<Users>(userJson);

            return users.Accounts.FirstOrDefault(u => u.Email.Equals(email));
        }

        public async Task<List<Entities.User>> GetUsers()
        {
            var userJson = await File.ReadAllTextAsync(_dbPath);

            var users = JsonConvert.DeserializeObject<Users>(userJson);

            return users.Accounts;
        }
    }
}
