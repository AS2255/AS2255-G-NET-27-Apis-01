using E_Commerce.Domain.Common;
using E_Commerce.Domain.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Specifications
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> inputQuery, ISpecification<TEntity, TKey> specs) where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;
            
            if (specs.IncludeExpression.Any())
            {
               query = specs.IncludeExpression.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            }


            return query;

        }
    }
}
