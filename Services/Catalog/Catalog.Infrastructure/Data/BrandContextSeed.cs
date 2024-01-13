using System.Reflection;
using System.Text.Json;
using Catalog.Core.Entities;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data;

public static class BrandContextSeed
{
    public static void SeedData(IMongoCollection<ProductBrand> brandCollection, IHostEnvironment env)
    {
        bool checkBrands = brandCollection.Find(b => true).Any();
        string path = Path.Combine(env.ContentRootPath, "db", "brands.json");
       
        if (!checkBrands)
        {
            var brandsData = File.ReadAllText(path);
            var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
            if (brands != null)
            {
                foreach (var item in brands)
                {
                    brandCollection.InsertOneAsync(item);
                }
            }
        }
    } 
}