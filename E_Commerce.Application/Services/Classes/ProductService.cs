using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Products;
using E_Commerce.Application.Services.Contracts;
using E_Commerce.Application.Specifications;
using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services.Classes
{
    public class ProductService(IUnitOfWork unitOfWork, IMapper mapper) : IProductService
    {
        public async Task<Result<IReadOnlyList<BrandDto>>> GetAllBrandsAsync(CancellationToken ct = default)
        {
            var brands = await unitOfWork.GetRepository<ProductBrand, int>().GetAllAsync(ct);
            var brandsDtos = mapper.Map<IReadOnlyList<BrandDto>>(brands);
            return Result<IReadOnlyList<BrandDto>>.Ok(brandsDtos);
        }

        public async Task<Result<IReadOnlyList<ProductDto>>> GetAllProductsAsync(CancellationToken ct = default)
        {
            var specs = new ProductWithBrandAndTypeSpecifications();

            var products = await unitOfWork.GetRepository<Product, int>().GetAllAsync(specs, ct);

            var productsDtos = mapper.Map<IReadOnlyList<ProductDto>>(products);
            return Result<IReadOnlyList<ProductDto>>.Ok(productsDtos);
        }

        public async Task<Result<IReadOnlyList<TypeDto>>> GetAllTypesAsync(CancellationToken ct = default)
        {
            var types = await unitOfWork.GetRepository<ProductType, int>().GetAllAsync(ct);
            var typesDtos = mapper.Map<IReadOnlyList<TypeDto>>(types);
            return Result<IReadOnlyList<TypeDto>>.Ok(typesDtos);
        }

        public async Task<Result<ProductDto>> GetProductByIdAsync(int id, CancellationToken ct = default)
        {
            var product = await unitOfWork.GetRepository<Product, int>().GetByIdAsync(id, ct);
            if(product is null)
            {
                return Result<ProductDto>.Fail(new Error("ProductNotFound", $"Product with id {id} not found."));
            }

            var productDto = mapper.Map<ProductDto>(product);

            return Result<ProductDto>.Ok(productDto);
        }
    }
}
