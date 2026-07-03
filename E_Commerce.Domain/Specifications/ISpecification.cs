using E_Commerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Domain.Specifications
{
    public interface ISpecification<IEntity, TKey> where IEntity : BaseEntity<TKey>
    {
         ICollection<Expression<Func<IEntity, object>>> IncludeExpression { get; }
        Expression<Func<IEntity, bool>> Criteria { get; }
        Expression<Func<IEntity, object>>? OrderBy { get; }
        Expression<Func<IEntity, object>>? OrderByDescending { get; }

    }
}
