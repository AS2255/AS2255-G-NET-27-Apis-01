using E_Commerce.Application.Common;
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
        public ProductWithBrandAndTypeSpecifications(ProductQueryParams queryParams) : 
            base
            (
                P =>
                (!queryParams.BrandId.HasValue || P.BrandId == queryParams.BrandId)
                &&
                (!queryParams.TypeId.HasValue || P.TypeId == queryParams.TypeId)
                &&
                (string.IsNullOrWhiteSpace(queryParams.SearchValue) || P.Name.ToLower().Contains(queryParams.SearchValue.ToLower()))
            )
        {
            AddInclude(P => P.Brand);
            AddInclude(P => P.Type);

            switch (queryParams.Sort)
            {
                case ProductSortOption.NameAsc:
                    AddOrderBy(P => P.Name);
                    break;
                case ProductSortOption.NameDesc:
                    AddOrderByDescending(P => P.Name);
                    break;
                case ProductSortOption.PriceAsc:
                    AddOrderBy(P => P.Price);
                    break;
                case ProductSortOption.PriceDesc:
                    AddOrderByDescending(P => P.Price);
                    break;
                default:
                    AddOrderBy(P => P.Id);
                    break;
            }
        }

        public ProductWithBrandAndTypeSpecifications(int id) : base(P => P.Id == id)
        {
            AddInclude(P => P.Brand);
            AddInclude(P => P.Type);
        }
    }
}
