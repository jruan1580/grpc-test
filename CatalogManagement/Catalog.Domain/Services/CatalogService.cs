using Catalog.Domain.Models;
using Catalog.Infrastructure.GrpcServices;
using Catalog.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Domain.Services
{
    public interface ICatalogService
    {
        Task<Item> GetCatalogItemById(string id);
        Task<List<Item>> GetItemsByCategory(string category);
        Task<List<Item>> GetItemsBySellerEmail(string email);
        Task AddItem(List<Item> items);
        Task UpdateQuantity(string itemId, string method, int quantity);
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

        public async Task<Item> GetCatalogItemById(string id)
        {
            if (!Guid.TryParse(id, out var itemId))
            {
                throw new ArgumentException($"Invalid Guid: {id}");
            }

            var catalogItem = await _catalogRepository.GetCatalogById(itemId);
            if (catalogItem == null)
            {
                throw new Exception($"Unable to locate item with id: {id}");
            }

            var seller = await _userService.GetUserById(catalogItem.SellerId.ToString());
            if (seller == null)
            {
                throw new Exception($"Unable to locate seller with id: {catalogItem.SellerId.ToString()} for catalog item with id: {catalogItem.Id.ToString()}");
            }

            return new Item()
            {
                Id = itemId,
                ItemName = catalogItem.ItemName,
                Category = catalogItem.Category,
                Quantity = catalogItem.Quantity,
                Price = catalogItem.Price,
                SellerEmail = seller.Email,
                SellerName = seller.Name
            };
        }

        public async Task<List<Item>> GetItemsByCategory(string category)
        {
            var catalogItems = await _catalogRepository.GetCatalogsByCategroy(category);
            var items = new List<Item>();

            foreach(var catalogItem in catalogItems)
            {
                var seller = await _userService.GetUserById(catalogItem.SellerId.ToString());
                if (seller == null)
                {
                    continue;
                }

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
            if (seller == null)
            {
                throw new Exception($"Unable to locate seller with email: {email}");
            }

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

        public async Task UpdateQuantity(string itemId, string operation, int quantity)
        {
            if (!Guid.TryParse(itemId, out var id))
            {
                throw new ArgumentException($"Invalid Guid: {itemId}");    
            }

            var catalogItem = await _catalogRepository.GetCatalogById(id);
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
