using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Specifications
{
    public class ProductWithBrandAndTypeSpecifications : BasedSpecification<Product, int>
    {
        public ProductWithBrandAndTypeSpecifications()
        {
            AddInclude(p => p.Brand);
            AddInclude(p => p.Type);
        }
    }
}
