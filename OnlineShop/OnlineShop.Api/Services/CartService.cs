using OnlineShop.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.Api.Services
{
    public interface ICartService
    {
        Task AddItemToCart(Guid userId, Guid itemId, int quantity);
        Task Checkout(Guid userId);
        Task<Models.Cart> GetUsersCart(Guid userId);
        Task ReduceItemQuantity(Guid userId, Guid itemId, int quantityToReduce);
        Task RemoveItemFromCart(Guid userId, Guid itemId);
    }

    public class CartService : ICartService
    {
        private readonly Cart.Grpc.Cart.CartClient _cartClient;

        public CartService(Cart.Grpc.Cart.CartClient cartClient)
        {
            _cartClient = cartClient;
        }

        public async Task<Models.Cart> GetUsersCart(Guid userId)
        {
            var usersCart = await _cartClient.GetUsersCartAsync(new Cart.Grpc.UserIdLookupModel() { UserId = userId.ToString() });

            var items = new List<ItemInCart>();
            foreach (var item in usersCart.Items)
            {
                items.Add(new ItemInCart()
                {
                    ItemId = Guid.Parse(item.ItemId),
                    ItemName = item.ItemName,
                    Category = item.Category,
                    QuantityInCart = item.QuantityInCart,
                    TotalPrice = (decimal)item.TotalPrice
                });
            }

            return new Models.Cart()
            {
                UserId = Guid.Parse(usersCart.UserId),
                UserName = usersCart.UserName,
                TotalAmount = (decimal)usersCart.TotalAmount,
                Items = items
            };
        }

        public async Task Checkout(Guid userId)
        {
            await _cartClient.CheckoutAsync(new Cart.Grpc.UserIdLookupModel() { UserId = userId.ToString() });
        }

        public async Task AddItemToCart(Guid userId, Guid itemId, int quantity)
        {
            await _cartClient.AddItemToCartAsync(new Cart.Grpc.AddItemToCartRequest() { UserId = userId.ToString(), ItemId = itemId.ToString(), Quantity = quantity });
        }

        public async Task RemoveItemFromCart(Guid userId, Guid itemId)
        {
            await _cartClient.RemoveItemFromCartAsync(new Cart.Grpc.RemoveItemFromCartRequest() { UserId = userId.ToString(), ItemId = itemId.ToString() });
        }

        public async Task ReduceItemQuantity(Guid userId, Guid itemId, int quantityToReduce)
        {
            await _cartClient.ReduceItemQuantityAsync(new Cart.Grpc.ReduceItemQuantityRequest() { UserId = userId.ToString(), ItemId = itemId.ToString(), ReduceQuantityAmount = quantityToReduce });
        }
    }
}
