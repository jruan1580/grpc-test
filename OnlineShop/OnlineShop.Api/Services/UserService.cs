using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.Api.Services
{
    public interface IUserService
    {
        Task CreateUser(string name, string email, string password);
        Task<Models.User> GetUserByEmail(string email);
        Task<Models.User> GetUserById(Guid userId);
        Task<List<Models.User>> GetUsers();
        Task Login(string email, string password);
        Task Logout(string email);
    }

    public class UserService : IUserService
    {
        private readonly User.Grpc.User.UserClient _userClient;

        public UserService(User.Grpc.User.UserClient userClient)
        {
            _userClient = userClient;
        }

        public async Task<List<Models.User>> GetUsers()
        {
            var users = new List<Models.User>();

            using (var stream = _userClient.GetUsers(new User.Grpc.EmptyModel()))
            {
                while (await stream.ResponseStream.MoveNext())
                {
                    var currentUser = stream.ResponseStream.Current;
                    users.Add(new Models.User()
                    {
                        Id = Guid.Parse(currentUser.Id),
                        Name = currentUser.Name,
                        Email = currentUser.Email,
                        LoggedOn = currentUser.LoggedOn
                    });
                }
            }

            return users;
        }

        public async Task<Models.User> GetUserById(Guid userId)
        {
            var user = await _userClient.GetUserByIdAsync(new User.Grpc.UserIdModel() { Id = userId.ToString() });

            return new Models.User()
            {
                Id = userId,
                Name = user.Name,
                Email = user.Email,
                LoggedOn = user.LoggedOn
            };
        }

        public async Task<Models.User> GetUserByEmail(string email)
        {
            var user = await _userClient.GetUserByEmailAsync(new User.Grpc.UserEmailModel() { Email = email });

            return new Models.User()
            {
                Id = Guid.Parse(user.Id),
                Name = user.Name,
                Email = user.Email,
                LoggedOn = user.LoggedOn
            };
        }

        public async Task CreateUser(string name, string email, string password)
        {
            var newUser = new User.Grpc.CreateUserModel()
            {
                User = new User.Grpc.UserModel()
                {
                    Name = name,
                    Email = email,
                    LoggedOn = false
                },
                Password = password
            };

            await _userClient.CreateUserAsync(newUser);
        }

        public async Task Login(string email, string password)
        {
            await _userClient.LoginAsync(new User.Grpc.LoginModel() { Email = email, Password = password });
        }

        public async Task Logout(string email)
        {
            await _userClient.LogoutAsync(new User.Grpc.UserEmailModel() { Email = email });
        }
    }
}
