using E_Commerce.Domain.Common;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Contracts.Repositories;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure
{
    public class UnitOfWork (StoreDbContext context) : IUnitOfWork
    {
        private readonly Dictionary<string, object> _repositories = [];
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var typeName = typeof(TEntity).Name;
            if (_repositories.TryGetValue(typeName, out object? value))
                return (IGenericRepository<TEntity, TKey>)value;
            
            var repo = new GenericRepository<TEntity, TKey>(context);
            _repositories.Add(typeName, repo);
            return repo;
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
            => await context.SaveChangesAsync(ct);
    }
}
