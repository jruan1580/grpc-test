using Cart.Domain.Services;
using Grpc.Core;
using System;
using System.Threading.Tasks;

namespace Cart.Grpc.Services
{
    public class CartService : Cart.CartBase
    {
        private readonly ICartService _coreCartService;
        public CartService(ICartService coreCartService)
        {
            _coreCartService = coreCartService;
        }

        public override async Task<EmptyModel> AddItemToCart(AddItemToCartRequest request, ServerCallContext context)
        {
            await _coreCartService.AddItemToCart(Guid.Parse(request.UserId), Guid.Parse(request.ItemId), request.Quantity);

            return new EmptyModel();            
        }

        public override async Task<EmptyModel> Checkout(UserIdLookupModel request, ServerCallContext context)
        {
            await _coreCartService.Checkout(Guid.Parse(request.UserId));

            return new EmptyModel();
        }

        public override Task<UsersCartResponse> GetUsersCart(UserIdLookupModel request, ServerCallContext context)
        {
            return base.GetUsersCart(request, context);
        }

        public override async Task<EmptyModel> ReduceItemQuantity(ReduceItemQuantityRequest request, ServerCallContext context)
        {
            await _coreCartService.ReduceItemQuantity(Guid.Parse(request.UserId), Guid.Parse(request.ItemId), request.ReduceQuantityAmount);

            return new EmptyModel();
        }

        public override async Task<EmptyModel> RemoveItemFromCart(RemoveItemFromCartRequest request, ServerCallContext context)
        {
            await _coreCartService.RemoveItemFromCart(Guid.Parse(request.UserId), Guid.Parse(request.ItemId));

            return new EmptyModel();
        }
    }
}
