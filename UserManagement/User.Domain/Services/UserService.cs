using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using User.Domain.Model;
using User.Infrastructure.Repository;

namespace User.Domain.Services
{
    public interface IUserService
    {
        Task<List<CoreUser>> GetUsers();
        Task<CoreUser> GetUserByEmail(string email);
        Task CreateUser(string email, string name, string password);
        Task Login(string email, string password);
        Task Logout(string email);
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordService _passwordService;

        public UserService(IUserRepository userRepository, IPasswordService passwordService)
        {
            _passwordService = passwordService;
            _userRepository = userRepository;
        }

        public async Task CreateUser(string email, string name, string password)
        {
            if (ValidEmail(email))
            {
                throw new ArgumentException($"Invalid email: {email}");
            }

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name is not passed in");
            }

            if (string.IsNullOrEmpty(password) || password.Length < 8 || password?.Length > 32)
            {
                throw new ArgumentException("Invalid password. Make sure password is between 8 - 32 characters.");
            }

            await _userRepository.CreateUser(name, email, _passwordService.CreatePasswordHash(password));
        }

        public async Task<CoreUser> GetUserByEmail(string email)
        {
            if (ValidEmail(email))
            {
                throw new ArgumentException($"Invalid email: {email}");
            }

            var user = await _userRepository.GetUserByEmail(email);

            if (user == null)
            {
                throw new Exception($"Unable to find user with email: {email}");
            }

            return new CoreUser() { Name = user.Name, Email = user.Email, LoggedOn = user.LoggedOn };
        }

        public async Task<List<CoreUser>> GetUsers()
        {
            var users = await _userRepository.GetUsers();
            var results = new List<CoreUser>();

            foreach(var user in users)
            {
                results.Add(new CoreUser() { Name = user.Name, Email = user.Email, LoggedOn = user.LoggedOn });
            }

            return results;
        }

        public async Task Login(string email, string password)
        {
            if (ValidEmail(email))
            {
                throw new ArgumentException($"Invalid email: {email}");
            }

            var user = await _userRepository.GetUserByEmail(email);

            if (user == null)
            {
                throw new Exception($"Unable to find user with email: {email}");
            }            

            if (!_passwordService.VerifyPasswordHash(password, user.Password))
            {
                throw new Exception($"Password is incorrect.");
            }

            await _userRepository.UpdateStatus(email, true);
        }

        public async Task Logout(string email)
        {
            if (ValidEmail(email))
            {
                throw new ArgumentException($"Invalid email: {email}");
            }

            var user = await _userRepository.GetUserByEmail(email);

            if (user == null)
            {
                throw new Exception($"Unable to find user with email: {email}");
            }

            await _userRepository.UpdateStatus(email, false);            
        }

        private bool ValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
