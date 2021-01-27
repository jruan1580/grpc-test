using System;

namespace OnlineShop.Api.Models
{
    public class Item
    {
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Category { get; set; }
		public int Quantity { get; set; }
		public decimal PricePerItem { get; set; }
		public string SellerEmail { get; set; }
		public string SellerName { get; set; }
	}
}
