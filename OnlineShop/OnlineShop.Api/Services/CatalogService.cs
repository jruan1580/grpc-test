namespace OnlineShop.Api.Services
{
    public class CatalogService
    {
        public Catalog.Grpc.Catalog.CatalogClient _catalogClient;

        public CatalogService(Catalog.Grpc.Catalog.CatalogClient catalogClient)
        {
            _catalogClient = catalogClient;
        }
    }
}
