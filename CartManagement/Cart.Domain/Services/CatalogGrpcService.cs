namespace Cart.Domain.Services
{
    public interface ICatalogGrpcService
    {

    }

    public class CatalogGrpcService : ICatalogGrpcService
    {
        private readonly Catalog.Grpc.Catalog.CatalogClient _catalogClient;

        public CatalogGrpcService(Catalog.Grpc.Catalog.CatalogClient catalogClient)
        {
            _catalogClient = catalogClient;
        }



    }
}
