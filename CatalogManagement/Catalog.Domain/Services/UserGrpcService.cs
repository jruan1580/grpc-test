using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using User.Grpc;

namespace Catalog.Domain.Services
{
    public interface IUserGrpcService
    {
        Task<UserModel> GetUserById(string userId);
        Task<UserModel> GetUserByEmail(string email);
    }

    public class UserGrpcService : IUserGrpcService
    {
        private readonly User.Grpc.User.UserClient _userClient;

        public UserGrpcService(IConfiguration configuration)
        {
            var address = configuration.GetSection("GrpcServices:UserURL").Value;
            var channel = GrpcChannel.ForAddress(address);

            _userClient = new User.Grpc.User.UserClient(channel);
        }

        public async Task<UserModel> GetUserById(string userId)
        {
            return await _userClient.GetUserByIdAsync(new UserIdModel() { Id = userId });
        }

        public async Task<UserModel> GetUserByEmail(string email)
        {
            return await _userClient.GetUserByEmailAsync(new UserEmailModel() { Email = email });
        }
    }
}
