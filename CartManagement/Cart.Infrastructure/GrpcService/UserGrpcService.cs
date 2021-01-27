using System;
using System.Threading.Tasks;

namespace Cart.Infrastructure.GrpcService
{
    public interface IUserGrpcService
    {
        Task<User.Grpc.UserModel> GetUserById(Guid userId);
    }

    public class UserGrpcService : IUserGrpcService
    {
        private readonly User.Grpc.User.UserClient _userClient;

        public UserGrpcService(User.Grpc.User.UserClient userClient)
        {
            _userClient = userClient;
        }

        public async Task<User.Grpc.UserModel> GetUserById(Guid userId)
        {
            return await _userClient.GetUserByIdAsync(new User.Grpc.UserIdModel() { Id = userId.ToString() });           
        }
    }
}
