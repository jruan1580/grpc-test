using Cart.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Cart.Domain.Services
{
    public interface ICartService
    {
        Task<CartByUser> GetUsersCart(Guid userId);
        Task AddItemToCart(Guid userId, Guid itemId, int qunatity);
        Task RemoveItemFromCart(Guid userId, Guid itemId);
        Task Checkout(Guid userId);
    }

    public class CartService : ICartService
    {
        public Task AddItemToCart(Guid userId, Guid itemId, int qunatity)
        {
            throw new NotImplementedException();
        }

        public Task Checkout(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<CartByUser> GetUsersCart(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveItemFromCart(Guid userId, Guid itemId)
        {
            throw new NotImplementedException();
        }
    }
}
