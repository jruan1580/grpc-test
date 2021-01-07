using Cart.Domain.Services;
using Google.Protobuf.Collections;
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

        public override async Task<UsersCartResponse> GetUsersCart(UserIdLookupModel request, ServerCallContext context)
        {
            var usersCart = await _coreCartService.GetUsersCart(Guid.Parse(request.UserId));

            var response = new UsersCartResponse()
            {
                UserId = usersCart.User.UserId.ToString(),
                UserName = usersCart.User.UserName,
                TotalAmount = (double)usersCart.TotalAmount
            };

            foreach (var itemInCart in usersCart.Items)
            {
                response.Items.Add(new ItemInCartModel()
                {
                    ItemId = itemInCart.Item.ItemId.ToString(),
                    ItemName = itemInCart.Item.ItemName,
                    Category = itemInCart.Item.Category,
                    QuantityInCart = itemInCart.QuantityInCart,
                    TotalPrice = (double)itemInCart.TotalPrice
                });;
            }

            return response;              
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
