using E_Commerce.API.Attributes;
using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Products;
using E_Commerce.Application.Services.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{

    public class ProductsController(IProductService productService) : ApiBaseController
    {
        //Get: api/products
        [HttpGet]
        [RedisCache(60)]
        public async Task<ActionResult<PaginationResult<ProductDto>>> GetAllProducts([FromQuery]ProductQueryParams queryParams , CancellationToken ct = default)
        {
            var result = await productService.GetAllProductsAsync(queryParams, ct);
            return ToActionResult(result);
        }

        //Get: api/products
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<BrandDto>>> GetAllBrands(CancellationToken ct = default)
        {
            var result = await productService.GetAllBrandsAsync(ct);
            return ToActionResult(result);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<TypeDto>>> GetAllTypes(CancellationToken ct = default)
        {
            var result = await productService.GetAllTypesAsync(ct);
            return ToActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id,CancellationToken ct = default)
        {
            var result = await productService.GetProductByIdAsync(id, ct);
            return ToActionResult(result);
        }
    }
}
