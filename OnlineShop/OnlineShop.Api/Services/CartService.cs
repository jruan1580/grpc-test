namespace OnlineShop.Api.Services
{
    public class CartService
    {
        private readonly Cart.Grpc.Cart.CartClient _cartClient;

        public CartService(Cart.Grpc.Cart.CartClient cartClient)
        {
            _cartClient = cartClient;
        }
    }
}
