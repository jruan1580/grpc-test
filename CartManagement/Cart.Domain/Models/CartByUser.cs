using System;
using System.Collections.Generic;
using System.Text;

namespace Cart.Domain.Models
{
    public class CartByUser
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public decimal TotalAmount { get; set; }
        public List<ItemInCart> Items { get; set; }
    }
}
