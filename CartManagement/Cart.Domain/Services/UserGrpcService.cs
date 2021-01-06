using System;
using System.Threading.Tasks;

namespace Cart.Domain.Services
{
    public interface IUserGrpcService
    {
        Task<Models.User> GetUserById(Guid userId);
    }

    public class UserGrpcService : IUserGrpcService
    {
        private readonly User.Grpc.User.UserClient _userClient;

        public UserGrpcService(User.Grpc.User.UserClient userClient)
        {
            _userClient = userClient;
        }

        public async Task<Models.User> GetUserById(Guid userId)
        {
            var user = await _userClient.GetUserByIdAsync(new User.Grpc.UserIdModel() { Id = userId.ToString() });

            return new Models.User()
            {
                UserId = userId,
                UserName = user.Name,
                Email = user.Email
            };
        }
    }
}
