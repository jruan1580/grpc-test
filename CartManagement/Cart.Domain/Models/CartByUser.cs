using System;
using System.Collections.Generic;

namespace Cart.Domain.Models
{
    public class CartByUser
    {
        public User User { get; set; }
        public decimal TotalAmount { get; set; }
        public List<ItemInCart> Items { get; set; }
    }
}
