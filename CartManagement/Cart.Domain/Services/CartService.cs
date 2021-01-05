using Cart.Domain.Models;
using Cart.Infrastructure.Repository;
using Cart.Infrastructure.Repository.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.Domain.Services
{
    public interface ICartService
    {
        Task<CartByUser> GetUsersCart(Guid userId);
        Task AddItemToCart(Guid userId, Guid itemId, int qunatity);
        Task RemoveItemFromCart(Guid userId, Guid itemId);
        Task Checkout(Guid userId);
        Task ReduceItemQuantity(Guid userId, Guid itemId, int quanityToReduceBy);
    }

    public class CartService : ICartService
    {
        private readonly ICartsRepository _cartsRepository;
        private readonly ICatalogGrpcService _catalogGrpcService;
        private readonly IUserGrpcService _userGrpcService;

        public CartService(ICartsRepository cartsRepository,
            ICatalogGrpcService catalogGrpcService,
            IUserGrpcService userGrpcService)
        {
            _cartsRepository = cartsRepository;
            _catalogGrpcService = catalogGrpcService;
            _userGrpcService = userGrpcService;
        }

        public async Task AddItemToCart(Guid userId, Guid itemId, int qunatity)
        {
            var item = new Items()
            {
                ItemId = itemId,
                Quantity = qunatity
            };

            await _cartsRepository.AddCartItem(userId, item);
        }

        /// <summary>
        /// Check out removes all item of a users and removes users from list
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task Checkout(Guid userId)
        {
            await _cartsRepository.RemoveUserFromCart(userId);
        }

        public Task<CartByUser> GetUsersCart(Guid userId)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveItemFromCart(Guid userId, Guid itemId)
        {
            await _cartsRepository.RemoveItemFromCart(userId, itemId);
        }

        public async Task ReduceItemQuantity(Guid userId, Guid itemId, int quanityToReduceBy)
        {
            var usersItem = await _cartsRepository.GetCartItemsByUserId(userId);

            var itemInCart = usersItem.FirstOrDefault(i => i.ItemId == itemId);

            if (itemInCart == null)
            {
                throw new Exception("Unable to locate item");
            }

            //remove item entirely since we want to reduce by more than the actual quantity in cart
            if (itemInCart.Quantity <= quanityToReduceBy)
            {
                await _cartsRepository.RemoveItemFromCart(userId, itemInCart.ItemId);

                return;
            }

            //otherwise, update quantity
            await _cartsRepository.UpdateQuantityByItem(userId, itemId, (itemInCart.Quantity - quanityToReduceBy));
        }
    }
}
