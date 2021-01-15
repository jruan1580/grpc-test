using System;

namespace Cart.Domain.Mappers
{
    public static class UserGrpcMapper
    {
        public static Models.User GrpcUserModelToDomainUserModel(this User.Grpc.UserModel user)
        {
            if (user == null)
            {
                return null;
            }

            return new Models.User()
            {
                UserId = Guid.Parse(user.Id),
                UserName = user.Name,
                Email = user.Email
            };
        }
    }
}
