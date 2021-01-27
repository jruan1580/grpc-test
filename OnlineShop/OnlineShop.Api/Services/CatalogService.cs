using Grpc.Core;
using OnlineShop.Api.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.Api.Services
{
    public interface ICatalogService
    {
        Task AddItem(List<Item> items);
        Task AddQuantity(Guid itemId, int quantityToAdd);
        Task<Item> GetItemById(Guid itemId);
        Task<List<Item>> GetItemsByCategory(string category);
        Task<List<Item>> GetItemsBySellerEmail(string sellerEmail);
        Task SubtractQuantity(Guid itemId, int quantityToSubtract);
    }

    public class CatalogService : ICatalogService
    {
        public Catalog.Grpc.Catalog.CatalogClient _catalogClient;

        public CatalogService(Catalog.Grpc.Catalog.CatalogClient catalogClient)
        {
            _catalogClient = catalogClient;
        }

        public async Task<Item> GetItemById(Guid itemId)
        {
            var item = await _catalogClient.GetItemByIdAsync(new Catalog.Grpc.ItemByIdLookup() { Id = itemId.ToString() });

            return new Item()
            {
                Id = itemId,
                Name = item.Name,
                Category = item.Category,
                Quantity = item.Quantity,
                PricePerItem = (decimal)item.Price,
                SellerEmail = item.SellerEmail,
                SellerName = item.SellerName
            };
        }

        public async Task<List<Item>> GetItemsByCategory(string category)
        {
            var items = new List<Item>();
            using (var stream = _catalogClient.GetItemsByCategory(new Catalog.Grpc.ItemByCategoryLookup() { Category = category }))
            {
                while (await stream.ResponseStream.MoveNext())
                {
                    var currentItem = stream.ResponseStream.Current;
                    items.Add(new Item()
                    {
                        Id = Guid.Parse(currentItem.Id),
                        Name = currentItem.Name,
                        Category = currentItem.Category,
                        Quantity = currentItem.Quantity,
                        PricePerItem = (decimal)currentItem.Price,
                        SellerEmail = currentItem.SellerEmail,
                        SellerName = currentItem.SellerName
                    });
                }
            }

            return items;
        }

        public async Task<List<Item>> GetItemsBySellerEmail(string sellerEmail)
        {
            var items = new List<Item>();
            using (var stream = _catalogClient.GetItemsBySellerEmail(new Catalog.Grpc.ItemBySellerEmailLookup() { Email = sellerEmail }))
            {
                while (await stream.ResponseStream.MoveNext())
                {
                    var currentItem = stream.ResponseStream.Current;
                    items.Add(new Item()
                    {
                        Id = Guid.Parse(currentItem.Id),
                        Name = currentItem.Name,
                        Category = currentItem.Category,
                        Quantity = currentItem.Quantity,
                        PricePerItem = (decimal)currentItem.Price,
                        SellerEmail = currentItem.SellerEmail,
                        SellerName = currentItem.SellerName
                    });
                }
            }

            return items;
        }

        public async Task AddQuantity(Guid itemId, int quantityToAdd)
        {
            await _catalogClient.AddQuantityAsync(new Catalog.Grpc.AddQuantityToItem() { Id = itemId.ToString(), Quantity = quantityToAdd });
        }

        public async Task SubtractQuantity(Guid itemId, int quantityToSubtract)
        {
            await _catalogClient.SubtractQuantityAsync(new Catalog.Grpc.SubtractQuantityFromItem() { Id = itemId.ToString(), Quantity = quantityToSubtract });
        }

        public async Task AddItem(List<Item> items)
        {
            var newItemModel = new Catalog.Grpc.AddNewItemsModel();

            foreach (var item in items)
            {
                newItemModel.Item.Add(new Catalog.Grpc.Item()
                {
                    Name = item.Name,
                    Category = item.Category,
                    Quantity = item.Quantity,
                    Price = (double)item.PricePerItem,
                    SellerEmail = item.SellerEmail,
                    SellerName = item.SellerName
                });
            }

            await _catalogClient.AddNewItemsAsync(newItemModel);
        }
    }
}
