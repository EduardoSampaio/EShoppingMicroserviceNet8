using System.Text.Json;
using Catalog.Core.Entities;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data;

public class CatalogContextSeed
{
    public static void SeedData(IMongoCollection<Product> productCollection, IHostEnvironment env)
    {
        bool checkProducts = productCollection.Find(b => true).Any();
        string path = Path.Combine(env.ContentRootPath, "db", "brands.json");
        if (!checkProducts)
        {
            var productsData = File.ReadAllText(path);
            var products = JsonSerializer.Deserialize<List<Product>>(productsData);
            if (products != null)
            {
                foreach (var item in products)
                {
                    productCollection.InsertOneAsync(item);
                }
            }
        }
    }
}