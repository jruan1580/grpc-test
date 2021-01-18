using System;

namespace OnlineShop.Api.Models
{
    public class ItemInCart
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public string Category { get; set; }
        public int QuantityInCart { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
