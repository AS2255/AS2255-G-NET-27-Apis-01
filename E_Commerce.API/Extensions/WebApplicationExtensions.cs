using E_Commerce.Domain.Contracts;
using System.Runtime.CompilerServices;

namespace E_Commerce.API.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task<WebApplication> SeedingAndMigrationDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var dataseeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Catalog");
            await dataseeder.SeedDataAsync();

            return app;
        }
    }
}
