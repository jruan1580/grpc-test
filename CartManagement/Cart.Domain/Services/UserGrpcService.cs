namespace Cart.Domain.Services
{
    public interface IUserGrpcService
    {

    }

    public class UserGrpcService : IUserGrpcService
    {
        private readonly User.Grpc.User.UserClient _userClient;

        public UserGrpcService(User.Grpc.User.UserClient userClient)
        {
            _userClient = userClient;
        }


    }
}
