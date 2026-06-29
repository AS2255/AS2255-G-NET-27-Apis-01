using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Products;
using E_Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.DataSeeding
{
    public class CatalogDataSeeder(StoreDbContext context, ILogger<CatalogDataSeeder> logger) : IDataSeeder
    {
        public async Task SeedDataAsync(CancellationToken ct = default)
        {
            try
            {
                // Check Database
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
                if (pendingMigrations.Any())
                {
                    await context.Database.MigrateAsync();
                }

                //seeding
                var SeedRootPath = Path.Combine(AppContext.BaseDirectory, "DataSeed");
                // Brand
                await SeedIfEmpty<ProductBrand, int>(SeedRootPath, "brands.json", ct);
                //Type
                await SeedIfEmpty<ProductType, int>(SeedRootPath, "types.json", ct);
                //Product
                await SeedIfEmpty<Product, int>(SeedRootPath, "products.json", ct);

                var count = await context.SaveChangesAsync(ct);
                if (count > 0)
                    logger.LogInformation($"{count} Rows Added to Database.");
                else
                    logger.LogInformation("No Rows Added to Database.");

            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                logger.LogError(ex.Message);
            }
        }

        private async Task SeedIfEmpty<TEntity, TKey> (string rootpath, string filename, CancellationToken ct = default) where TEntity : BaseEntity<TKey>
        {
            if (await context.Set<TEntity>().AnyAsync())
            {
                var tableName = typeof(TEntity).Name;
                logger.LogWarning($"Table {tableName} Already Has Data");

                return;
            }

            var filePath = Path.Combine(rootpath, filename);
            if (!File.Exists(filePath)) 
            {
                logger.LogWarning($"File {filename}  Not Exist.");
                return;
            }

            using var fileStream = File.OpenRead(filePath);

            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            var data = await JsonSerializer.DeserializeAsync<List<TEntity>>(fileStream, options, ct);

            if (data is not null && data.Any())
                await context.Set<TEntity>().AddRangeAsync(data);

        }
    }
}
