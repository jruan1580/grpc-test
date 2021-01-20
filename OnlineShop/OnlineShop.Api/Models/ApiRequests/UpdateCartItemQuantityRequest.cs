using System;

namespace OnlineShop.Api.Models.ApiRequests
{
    public class UpdateCartItemQuantityRequest : UpdateItemQuantityRequest
    {
        public Guid UserId { get; set; }
    }
}
