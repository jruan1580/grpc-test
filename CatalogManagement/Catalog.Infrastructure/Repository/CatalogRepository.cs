using Catalog.Infrastructure.Repository.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Repository
{
    public interface ICatalogRepository
    {
        Task<Entities.Catalog> GetCatalogById(Guid itemId);
        Task<List<Entities.Catalog>> GetCatalogsByCategroy(string category);
        Task<List<Entities.Catalog>> GetCatalogsBySeller(Guid sellerId);
        Task AddCatalogItems(List<Entities.Catalog> catalogs);
        Task UpdateCatalog(Entities.Catalog catalogToUpdate);
    }

    public class CatalogRepository : ICatalogRepository
    {
        private readonly string _dbPath;
        public CatalogRepository()
        {
            var outPutDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

            var path = Path.Combine(outPutDirectory, "Repository\\Database\\Catalog.txt");

            if (File.Exists(path))
            {
                throw new Exception("Unable to locate catalog db");
            }

            _dbPath = path;
        }

        public async Task AddCatalogItems(List<Entities.Catalog> catalogs)
        {
            var catalogJson = await File.ReadAllTextAsync(_dbPath);

            var items = JsonConvert.DeserializeObject<Catalogs>(catalogJson);
            var itemsToAdd = new List<Entities.Catalog>();
            //loop through for update
            foreach(var item in items.CatalogItems)
            {
                var existingItem = items.CatalogItems.FirstOrDefault(c => c.ItemName == item.ItemName && c.SellerId == item.SellerId);
                if (existingItem != null)
                {
                    //update quantity
                    existingItem.Quantity += item.Quantity;
                    continue;
                }

                //add item to list of items to add later
                item.Id = Guid.NewGuid();
                itemsToAdd.Add(item);
            }

            items.CatalogItems.AddRange(itemsToAdd);

            await File.WriteAllTextAsync(_dbPath, JsonConvert.SerializeObject(items));
        }

        public async Task<Entities.Catalog> GetCatalogById(Guid itemId)
        {
            var catalogJson = await File.ReadAllTextAsync(_dbPath);

            var items = JsonConvert.DeserializeObject<Catalogs>(catalogJson);

            return items.CatalogItems.FirstOrDefault(c => c.Id == itemId);
        }

        public async Task<List<Entities.Catalog>> GetCatalogsByCategroy(string category)
        {
            var catalogJson = await File.ReadAllTextAsync(_dbPath);

            var items = JsonConvert.DeserializeObject<Catalogs>(catalogJson);

            return items.CatalogItems.Where(c => c.Category.Equals(category)).ToList();
        }

        public async Task<List<Entities.Catalog>> GetCatalogsBySeller(Guid sellerId)
        {
            var catalogJson = await File.ReadAllTextAsync(_dbPath);

            var items = JsonConvert.DeserializeObject<Catalogs>(catalogJson);

            return items.CatalogItems.Where(c => c.SellerId == sellerId).ToList();
        }

        public async Task UpdateCatalog(Entities.Catalog catalogToUpdate)
        {
            var catalogJson = await File.ReadAllTextAsync(_dbPath);

            var items = JsonConvert.DeserializeObject<Catalogs>(catalogJson);

            var existingItem = items.CatalogItems.FirstOrDefault(c => c.Id == catalogToUpdate.Id);

            //nothing to update
            if (existingItem == null)
            {
                return;
            }

            //update obj
            existingItem.ItemName = catalogToUpdate.ItemName;
            existingItem.Category = catalogToUpdate.Category;
            existingItem.Price = catalogToUpdate.Price;
            existingItem.Quantity = catalogToUpdate.Quantity;
            existingItem.SellerId = catalogToUpdate.SellerId;

            //write entire json back to file with new updated item
            await File.WriteAllTextAsync(_dbPath, JsonConvert.SerializeObject(items));
        }
    }
}
