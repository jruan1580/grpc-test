using System;
using System.Collections.Generic;

namespace Cart.Infrastructure.Repository.Entities
{
    public class UsersCart
    {
        public Guid UserId { get; set; }

        public List<Items> Items { get; set; }
    }
}
