using Grpc.Core;
using System;
using System.Threading.Tasks;
using User.Domain.Services;

namespace User.Grpc.Services
{
    public class UserService : User.UserBase
    {
        private readonly IUserService _userService;
        public UserService(IUserService userService)
        {
            _userService = userService;
        }

        public override async Task GetUsers(EmptyModel request, IServerStreamWriter<UserModel> responseStream, ServerCallContext context)
        {
            try
            {
                var users = await _userService.GetUsers();

                foreach (var user in users)
                {
                    await responseStream.WriteAsync(new UserModel() { Id = user.UserId.ToString(), Name = user.Name, Email = user.Email, LoggedOn = user.LoggedOn });
                }
            }
            catch(Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message), ex.Message);
            }                 
        }

        public override async Task<UserModel> GetUserById(UserIdModel request, ServerCallContext context)
        {
            try
            {
                var user = await _userService.GetUserById(request.Id);

                return new UserModel() { Id = user.UserId.ToString(), Name = user.Name, Email = user.Email, LoggedOn = user.LoggedOn };
            }
            catch(ArgumentException argEx)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, argEx.Message), argEx.Message);
            }
            catch(Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message), ex.Message);
            }         
        }

        public override async Task<UserModel> GetUserByEmail(UserEmailModel request, ServerCallContext context)
        {
            try
            {
                var user = await _userService.GetUserByEmail(request.Email);

                return new UserModel() { Id = user.UserId.ToString(), Name = user.Name, Email = user.Email, LoggedOn = user.LoggedOn };
            }
            catch (ArgumentException argEx)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, argEx.Message), argEx.Message);
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message), ex.Message);
            }           
        }

        public override async Task<EmptyModel> CreateUser(CreateUserModel request, ServerCallContext context)
        {
            try
            {
                await _userService.CreateUser(request.User.Email, request.User.Name, request.Password);

                return new EmptyModel();
            }
            catch(ArgumentException argEx)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument , argEx.Message), argEx.Message);
            }
            catch(Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message), ex.Message);
            }
        }

        public override async Task<EmptyModel> Login(LoginModel request, ServerCallContext context)
        {
            try
            {
                await _userService.Login(request.Email, request.Password);

                return new EmptyModel();
            }
            catch (ArgumentException argEx)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, argEx.Message), argEx.Message);
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message), ex.Message);
            }           
        }

        public override async Task<EmptyModel> Logout(UserEmailModel request, ServerCallContext context)
        {
            try
            {
                await _userService.Logout(request.Email);

                return new EmptyModel();
            }
            catch (ArgumentException argEx)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, argEx.Message), argEx.Message);
            }
            catch (Exception ex)
            {
                throw new RpcException(new Status(StatusCode.Internal, ex.Message), ex.Message);
            }         
        }
    }
}
