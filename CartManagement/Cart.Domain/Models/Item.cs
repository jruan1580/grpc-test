using System;

namespace Cart.Domain.Models
{
    public class Item
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public string Category { get; set; }
        public string SellerName { get; set; }
        public decimal PricePerItem { get; set; }
    }
}
