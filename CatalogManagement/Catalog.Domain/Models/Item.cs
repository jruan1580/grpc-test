using System;

namespace Catalog.Domain.Models
{
    public class Item
    {
        public Guid Id { get; set; }
        public string ItemName { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string SellerEmail { get; set; }
        public string SellerName { get; set; }
    }
}
