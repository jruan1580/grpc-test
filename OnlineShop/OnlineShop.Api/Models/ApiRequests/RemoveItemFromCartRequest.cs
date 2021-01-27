using System;

namespace OnlineShop.Api.Models.ApiRequests
{
    public class RemoveItemFromCartRequest
    {
        public Guid UserId { get; set; }
        public Guid ItemId { get; set; }
    }
}
