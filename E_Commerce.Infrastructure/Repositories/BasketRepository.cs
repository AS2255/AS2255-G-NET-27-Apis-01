using E_Commerce.Domain.Contracts.Repositories;
using E_Commerce.Domain.Entities.Baskets;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Repositories
{
    public class BasketRepository(IConnectionMultiplexer connection) : IBasketRepository
    {
        private readonly IDatabase _database= connection.GetDatabase();
        public async Task<CustomerBasket?> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? TimeToLive = null, CancellationToken ct = default)
        {
            var value = JsonSerializer.Serialize(basket);

           var result =await _database.StringSetAsync(basket.Id, value, TimeToLive?? TimeSpan.FromDays(7));

            return result ? basket: null;
        }
        public async Task<bool> DeleteBasketAsync(string basketId, CancellationToken ct = default)
        {
            return await _database.KeyDeleteAsync(basketId);
        }
        public async Task<CustomerBasket?> GetBasketAsync(string basketId, CancellationToken ct = default)
        {
            var value = await _database.StringGetAsync(basketId);
            if (value.IsNullOrEmpty) return null;
            var basket = JsonSerializer.Deserialize<CustomerBasket>(value!);
            if (basket is null) return null;
            return basket;
        }
    }
}
