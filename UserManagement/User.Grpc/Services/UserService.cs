using Grpc.Core;
using System.Threading.Tasks;

namespace User.Grpc.Services
{
    public class UserService : User.UserBase
    {
        public override Task GetUsers(EmptyModel request, IServerStreamWriter<UserModel> responseStream, ServerCallContext context)
        {
            return base.GetUsers(request, responseStream, context);
        }

        public override Task<UserModel> GetUserByEmail(UserByEmailLookupModel request, ServerCallContext context)
        {
            return base.GetUserByEmail(request, context);
        }

        public override Task<CreateUserResponseModel> CreateUser(UserModel request, ServerCallContext context)
        {
            return base.CreateUser(request, context);
        }
    }
}
