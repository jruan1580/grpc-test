using System;
using System.Threading.Tasks;

namespace Cart.Domain.Services
{
    public interface ICatalogGrpcService
    {
        Task<Catalog.Grpc.Item> GetItemById(Guid itemId);
    }

    public class CatalogGrpcService : ICatalogGrpcService
    {
        private readonly Catalog.Grpc.Catalog.CatalogClient _catalogClient;

        public CatalogGrpcService(Catalog.Grpc.Catalog.CatalogClient catalogClient)
        {
            _catalogClient = catalogClient;
        }

        public async Task<Catalog.Grpc.Item> GetItemById(Guid itemId)
        {
            return await _catalogClient.GetItemByIdAsync(new Catalog.Grpc.ItemByIdLookup() { Id = itemId.ToString() });
        }
    }
}
