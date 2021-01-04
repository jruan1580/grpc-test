using Cart.Infrastructure.Repository.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Cart.Infrastructure.Repository
{
    public interface ICartsRepository
    {
        Task<List<Items>> GetCartItemsByUserId(Guid userId);
        Task AddCartItem(Guid userId, Items item);
        Task RemoveItemFromCart(Guid userId, Guid itemId);
        Task UpdateQuantityByItem(Guid userId, Guid itemId, int quantity);
    }

    public class CartsRepository : ICartsRepository
    {
        private readonly string _dbPath;
        public CartsRepository()
        {
            var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

            var path = Path.Combine(outPutDirectory, "Repository\\Database\\CartItems.txt");

            if (File.Exists(path))
            {
                throw new Exception("Unable to locate cartitems db");
            }

            _dbPath = path;
        }

        public async Task AddCartItem(Guid userId, Items newItem)
        {
            var cartJson = await File.ReadAllTextAsync(_dbPath);
            
            var cartDb = JsonConvert.DeserializeObject<Carts>(cartJson);

            var usersCart = cartDb.DbRecords.FirstOrDefault(c => c.UserId == userId);

            //user does not exist meaning first time check out, add user and new item to db
            if (usersCart == null)
            {
                var newCartRecord = new UsersCart()
                {
                    UserId = userId,
                    Items = new List<Items>() { newItem }
                };

                cartDb.DbRecords.Add(newCartRecord);

                await File.WriteAllTextAsync(_dbPath, JsonConvert.SerializeObject(cartDb));

                return;
            }
      
            //user has existing items in an existing cart.
            //find item and update quantity if item exists, other wise, add item to user cart
            var itemStored = usersCart.Items.FirstOrDefault(i => i.ItemId == newItem.ItemId);

            if (itemStored != null)
            {
                itemStored.Quantity += newItem.Quantity;
            }
            else
            {
                usersCart.Items.Add(newItem);
            }

            await File.WriteAllTextAsync(_dbPath, JsonConvert.SerializeObject(cartDb));
        }

        public async Task<List<Items>> GetCartItemsByUserId(Guid userId)
        {
            var cartJson = await File.ReadAllTextAsync(_dbPath);

            var cartDb = JsonConvert.DeserializeObject<Carts>(cartJson);

            var usersCart = cartDb.DbRecords.FirstOrDefault(c => c.UserId == userId);

            if (usersCart == null)
            {
                throw new ArgumentException($"Unable to find cart for userid: {userId}");
            }

            return usersCart.Items;
        }

        public async Task RemoveItemFromCart(Guid userId, Guid itemId)
        {
            var cartJson = await File.ReadAllTextAsync(_dbPath);

            var cartDb = JsonConvert.DeserializeObject<Carts>(cartJson);

            var usersCart = cartDb.DbRecords.FirstOrDefault(c => c.UserId == userId);

            if (usersCart == null)
            {
                throw new ArgumentException($"Unable to find cart for userid: {userId}");
            }

            var itemStored = usersCart.Items.FirstOrDefault(i => i.ItemId == itemId);

            if (itemStored == null)
            {
                throw new ArgumentException($"Unable to locate item:{itemId} for user {userId}");
            }

            usersCart.Items.Remove(itemStored);

            //no more item, remove users cart record
            if (usersCart.Items.Count <= 0)
            {
                cartDb.DbRecords.Remove(usersCart);
            }

            await File.WriteAllTextAsync(_dbPath, JsonConvert.SerializeObject(cartDb));
        }

        public async Task UpdateQuantityByItem(Guid userId, Guid itemId, int quantity)
        {
            var cartJson = await File.ReadAllTextAsync(_dbPath);

            var cartDb = JsonConvert.DeserializeObject<Carts>(cartJson);

            var usersCart = cartDb.DbRecords.FirstOrDefault(c => c.UserId == userId);

            if (usersCart == null)
            {
                throw new ArgumentException($"Unable to find cart for userid: {userId}");
            }

            var itemStored = usersCart.Items.FirstOrDefault(i => i.ItemId == itemId);

            if (itemStored == null)
            {
                throw new ArgumentException($"Unable to locate item:{itemId} for user {userId}");
            }

            itemStored.Quantity = quantity;

            await File.WriteAllTextAsync(_dbPath, JsonConvert.SerializeObject(cartDb));
        }
    }
}
