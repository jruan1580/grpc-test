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
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
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

            await _userRepository.CreateUser(name, email, password);
        }

        public Task<CoreUser> GetUserByEmail(string email)
        {
            if (ValidEmail(email))
            {
                throw new ArgumentException($"Invalid email: {email}");
            }

            throw new System.NotImplementedException();
        }

        public Task<List<CoreUser>> GetUsers()
        {
            throw new System.NotImplementedException();
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

        //private byte []
    }
}
