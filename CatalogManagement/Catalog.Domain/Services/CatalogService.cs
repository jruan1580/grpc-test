using Catalog.Domain.Models;
using Catalog.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain.Services
{
    public interface ICatalogService
    {
        Task<List<Item>> GetItemsByCategory(string category);
        Task<List<Item>> GetItemsBySellerEmail(string email);
        Task AddItem(List<Item> items);
        Task UpdateQuantity(Guid itemId, string method, int quantity);
    }

    public class CatalogService : ICatalogService
    {
        private ICatalogRepository _catalogRepository;
        private IUserGrpcService _userService;

        public CatalogService(ICatalogRepository catalogRepository, IUserGrpcService userService)
        {
            _catalogRepository = catalogRepository;
            _userService = userService;
        }

        public async Task AddItem(List<Item> items)
        {
            var catalogItems = new List<Infrastructure.Repository.Entities.Catalog>();

            foreach (var item in items)
            {
                var seller = await _userService.GetUserByEmail(item.SellerEmail);

                catalogItems.Add(new Infrastructure.Repository.Entities.Catalog()
                {
                    ItemName = item.ItemName,
                    Category = item.Category,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    SellerId = Guid.Parse(seller.Id)
                });
            }

            await _catalogRepository.AddCatalogItems(catalogItems);
        }

        public async Task<List<Item>> GetItemsByCategory(string category)
        {
            var catalogItems = await _catalogRepository.GetCatalogsByCategroy(category);
            var items = new List<Item>();

            foreach(var catalogItem in catalogItems)
            {
                var seller = await _userService.GetUserById(catalogItem.SellerId.ToString());
                items.Add(new Item()
                {
                    Id = catalogItem.Id,
                    ItemName = catalogItem.ItemName,
                    Category = catalogItem.Category,
                    Quantity = catalogItem.Quantity,
                    Price = catalogItem.Price,
                    SellerEmail = seller.Email,
                    SellerName = seller.Name
                });
            }

            return items;
        }

        public async Task<List<Item>> GetItemsBySellerEmail(string email)
        {
            var seller = await _userService.GetUserByEmail(email);
            var catalogItems = await _catalogRepository.GetCatalogsBySeller(Guid.Parse(seller.Id));
            var items = new List<Item>();

            foreach (var catalogItem in catalogItems)
            {
                items.Add(new Item()
                {
                    Id = catalogItem.Id,
                    ItemName = catalogItem.ItemName,
                    Category = catalogItem.Category,
                    Quantity = catalogItem.Quantity,
                    Price = catalogItem.Price,
                    SellerEmail = seller.Email,
                    SellerName = seller.Name
                });
            }

            return items;
        }

        public async Task UpdateQuantity(Guid itemId, string operation, int quantity)
        {
            var catalogItem = await _catalogRepository.GetCatalogById(itemId);
            if (catalogItem == null)
            {
                throw new Exception($"Unable to locate item with id: {itemId}");
            }

            switch (operation)
            {
                case "add":
                    catalogItem.Quantity += quantity;
                    break;
                case "subtract":
                    catalogItem.Quantity -= quantity;
                    break;
                default:
                    throw new Exception("Unsupported operation");
            }

            await _catalogRepository.UpdateCatalog(catalogItem);
        }
    }
}
