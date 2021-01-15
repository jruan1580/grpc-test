using Cart.Domain.Models;
using System;

namespace Cart.Domain.Mappers
{
    public static class CatalogGrpcMapper
    {
        public static Item CatalogItemToDomainItem(this Catalog.Grpc.Item item)
        {
            if (item == null)
            {
                return null;
            }

            return new Item()
            {
                ItemId = Guid.Parse(item.Id),
                ItemName = item.Name,
                Category = item.Category,
                SellerName = item.SellerName,
                PricePerItem = (decimal)item.Price
            };
        }
    }
}
