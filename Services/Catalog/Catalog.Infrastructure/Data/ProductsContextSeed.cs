using Catalog.Core.Entities;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using System.Text.Json;

namespace Catalog.Infrastructure.Data;

public static class ProductsContextSeed
{
    public static void SeedData(IMongoCollection<Product> productCollection, IHostEnvironment env)
    {
        bool checkProduct = productCollection.Find(b => true).Any();
        string path = Path.Combine(env.ContentRootPath, "db", "products.json");

        if (!checkProduct)
        {
            var productData = File.ReadAllText(path);
            var products = JsonSerializer.Deserialize<List<Product>>(productData);

            if (products != null)
            {
                foreach (var product in products)
                {
                    productCollection.InsertOneAsync(product);
                }
            }
        }
    }
}


