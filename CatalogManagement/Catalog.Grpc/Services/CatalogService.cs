using Catalog.Domain.Services;
using Grpc.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Grpc.Services
{
    public class CatalogService : Catalog.CatalogBase
    {
        private readonly ICatalogService _catalogService;
        public CatalogService(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        public override async Task<EmptyModel> AddNewItems(AddNewItemsModel request, ServerCallContext context)
        {
            var items = new List<Domain.Models.Item>();
            foreach(var item in request.Item)
            {
                items.Add(new Domain.Models.Item()
                {
                    ItemName = item.Name,
                    Category = item.Category,
                    Quantity = item.Quantity,
                    Price = (decimal)item.Price,
                    SellerEmail = item.SellerEmail,
                    SellerName = item.SellerName
                });
            }

            await _catalogService.AddItem(items);

            return new EmptyModel();
        }

        public override async Task<EmptyModel> AddQuantity(AddQuantityToItem request, ServerCallContext context)
        {
            await _catalogService.UpdateQuantity(request.Id, "add", request.Quantity);

            return new EmptyModel();
        }

        public override async Task<Item> GetItemById(ItemByIdLookup request, ServerCallContext context)
        {
            var itemById = await _catalogService.GetCatalogItemById(request.Id);

            return new Item()
            {
                Id = itemById.Id.ToString(),
                Name = itemById.ItemName,
                Category = itemById.Category,
                Quantity = itemById.Quantity,
                Price = (double)itemById.Price,
                SellerEmail = itemById.SellerEmail,
                SellerName = itemById.SellerName
            };
        }

        public override async Task GetItemsByCategory(ItemByCategoryLookup request, IServerStreamWriter<Item> responseStream, ServerCallContext context)
        {
            var itemsByCategory = await _catalogService.GetItemsByCategory(request.Category);

            foreach(var item in itemsByCategory)
            {
                await responseStream.WriteAsync(new Item()
                {
                    Id = item.Id.ToString(),
                    Name = item.ItemName,
                    Category = item.Category,
                    Quantity = item.Quantity,
                    Price = (double)item.Price,
                    SellerEmail = item.SellerEmail,
                    SellerName = item.SellerName
                });
            }
        }

        public override async Task GetItemsBySellerEmail(ItemBySellerEmailLookup request, IServerStreamWriter<Item> responseStream, ServerCallContext context)
        {
            var itemsBySellerEmail = await _catalogService.GetItemsBySellerEmail(request.Email);

            foreach (var item in itemsBySellerEmail)
            {
                await responseStream.WriteAsync(new Item()
                {
                    Id = item.Id.ToString(),
                    Name = item.ItemName,
                    Category = item.Category,
                    Quantity = item.Quantity,
                    Price = (double)item.Price,
                    SellerEmail = item.SellerEmail,
                    SellerName = item.SellerName
                });
            }
        }

        public override async Task<EmptyModel> SubtractQuantity(SubtractQuantityFromItem request, ServerCallContext context)
        {
            await _catalogService.UpdateQuantity(request.Id, "add", request.Quantity);

            return new EmptyModel();
        }        
    }
}
