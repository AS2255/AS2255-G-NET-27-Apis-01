using E_Commerce.Domain.Common;
using E_Commerce.Domain.Entities.Products;
using E_Commerce.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    public class BasedSpecification <TEntity, TKey> : ISpecification<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpression { get; } = [];

        public Expression<Func<TEntity, bool>> Criteria { get; private set; }

        public BasedSpecification(Expression<Func<TEntity, bool>> expression)
        {
            Criteria = expression;
        }

        protected void AddInclude(Expression<Func<TEntity, object>> expression)
        {
            IncludeExpression.Add(expression);
        }
    }
}
