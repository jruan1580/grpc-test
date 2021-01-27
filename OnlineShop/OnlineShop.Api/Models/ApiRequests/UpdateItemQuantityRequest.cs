using System;

namespace OnlineShop.Api.Models.ApiRequests
{
    public class UpdateItemQuantityRequest
    {
        public Guid ItemId { get; set; }
        public int Quantity { get; set; }
        public string Operation { get; set; }
    }
}
