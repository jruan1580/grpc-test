using System;
using System.Collections.Generic;

namespace Catalog.Infrastructure.Repository.Entities
{
    public class Catalogs
    {
        public List<Catalog> CatalogItems { get; set; }
    }

    public class Catalog
    {
        public Guid Id { get; set; }
        public string ItemName { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Guid SellerId { get; set; }
    }
}
