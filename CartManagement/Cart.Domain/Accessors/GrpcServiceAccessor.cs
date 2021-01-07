using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;

namespace Cart.Domain.Accessors
{
    public interface IGrpcServiceAccessor
    {
        User.Grpc.User.UserClient GetUserGrpcClient();
        Catalog.Grpc.Catalog.CatalogClient GetCatalogGrpcClient();
    }

    public class GrpcServiceAccessor : IGrpcServiceAccessor
    {
        private readonly IConfiguration _configuration;
        public GrpcServiceAccessor(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Catalog.Grpc.Catalog.CatalogClient GetCatalogGrpcClient()
        {
            var catalogGrpcUrl = _configuration.GetSection("Grpc:Catalog").Value;

            var channel = GrpcChannel.ForAddress(catalogGrpcUrl);

            return new Catalog.Grpc.Catalog.CatalogClient(channel);
        }

        public User.Grpc.User.UserClient GetUserGrpcClient()
        {
            var userGrpcUrl = _configuration.GetSection("Grpc:User").Value;

            var channel = GrpcChannel.ForAddress(userGrpcUrl);

            return new User.Grpc.User.UserClient(channel);
        }
    }
}
