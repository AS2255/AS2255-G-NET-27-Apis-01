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
        public ProductWithBrandAndTypeSpecifications() : base(null)
        {
            AddInclude(P => P.Brand);
            AddInclude(P => P.Type);
        }

        public ProductWithBrandAndTypeSpecifications(int id) : base(P => P.Id == id)
        {
            AddInclude(P => P.Brand);
            AddInclude(P => P.Type);
        }
    }
}
