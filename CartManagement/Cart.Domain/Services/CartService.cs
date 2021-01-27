using Cart.Domain.Mappers;
using Cart.Domain.Models;
using Cart.Infrastructure.GrpcService;
using Cart.Infrastructure.Repository;
using Cart.Infrastructure.Repository.Entities;
using System;
using System.Collections.Generic;
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

        public async Task<CartByUser> GetUsersCart(Guid userId)
        {
            var user = (await _userGrpcService.GetUserById(userId)).GrpcUserModelToDomainUserModel(); ;

            if (user == null)
            {
                throw new ArgumentException($"Unable to locate user with id: {userId.ToString()}");
            }

            var items = await _cartsRepository.GetCartItemsByUserId(userId);

            var itemsInCart = new List<ItemInCart>();
            var totalAmount = 0m;

            foreach(var item in items)
            {
                var cartItem = (await _catalogGrpcService.GetItemById(item.ItemId)).CatalogItemToDomainItem();

                //remove it from cart repo
                if (cartItem == null)
                {
                    await _cartsRepository.RemoveItemFromCart(userId, item.ItemId);

                    continue;
                }
                
                var itemInCart = new ItemInCart()
                {
                    Item = cartItem,
                    QuantityInCart = item.Quantity,
                    TotalPrice = (item.Quantity * cartItem.PricePerItem)
                };

                totalAmount += itemInCart.TotalPrice;
                itemsInCart.Add(itemInCart);
            }

            return new CartByUser()
            {
                User = user,
                Items = itemsInCart,
                TotalAmount = totalAmount
            };
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
