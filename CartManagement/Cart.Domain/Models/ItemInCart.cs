using System;

namespace Cart.Domain.Models
{
    public class ItemInCart
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public string Category { get; set; }
        public string SellerName { get; set; }
        public string QuantityInCart { get; set; }
        public decimal Price { get; set; }
    }
}
