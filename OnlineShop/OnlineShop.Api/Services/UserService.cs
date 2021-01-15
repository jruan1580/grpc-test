using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.Api.Services
{
    public class UserService
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


    }
}
