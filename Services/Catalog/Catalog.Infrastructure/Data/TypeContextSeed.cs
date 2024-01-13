using System.Text.Json;
using Catalog.Core.Entities;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data;

public class TypeContextSeed
{
    public static void SeedData(IMongoCollection<ProductType> typeCollection, IHostEnvironment env)
    {
        bool checkTypes = typeCollection.Find(b => true).Any();
        string path = Path.Combine(env.ContentRootPath, "db", "types.json");

        if (!checkTypes)
        {
            var typesData = File.ReadAllText(path);
            var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
            if (types != null)
            {
                foreach (var item in types)
                {
                    typeCollection.InsertOneAsync(item);
                }
            }
        }
    }
}