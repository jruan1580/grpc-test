using System;
using System.Collections.Generic;

namespace OnlineShop.Api.Models
{
    public class Cart
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public decimal TotalAmount { get; set; }
        public List<ItemInCart> Items { get; set; }
    }
}
