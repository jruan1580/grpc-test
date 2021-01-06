using Cart.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Cart.Domain.Services
{
    public interface ICatalogGrpcService
    {
        Task<Item> GetItemById(Guid itemId);
    }

    public class CatalogGrpcService : ICatalogGrpcService
    {
        private readonly Catalog.Grpc.Catalog.CatalogClient _catalogClient;

        public CatalogGrpcService(Catalog.Grpc.Catalog.CatalogClient catalogClient)
        {
            _catalogClient = catalogClient;
        }

        public async Task<Item> GetItemById(Guid itemId)
        {
            var catalogItem = await _catalogClient.GetItemByIdAsync(new Catalog.Grpc.ItemByIdLookup() { Id = itemId.ToString() });

            return new Item()
            {
                ItemId = Guid.Parse(catalogItem.Id),
                ItemName = catalogItem.Name,
                Category = catalogItem.Category,
                SellerName = catalogItem.SellerName,
                PricePerItem = (decimal)catalogItem.Price
            }; 
        }

    }
}
