using AutoMapper;
using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Baskets;
using E_Commerce.Application.Services.Contracts;
using E_Commerce.Domain.Contracts.Repositories;
using E_Commerce.Domain.Entities.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services.Classes
{
    public class BasketService(IBasketRepository basketRepository, IMapper mapper) : IBasketService
    {
        public async Task<Result<BasketDto>> CreateOrUpdateBasketAsync(BasketDto dto, TimeSpan? timetolive = null, CancellationToken ct = default)
        {
            var basket = mapper.Map<CustomerBasket>(dto);
            var result = await basketRepository.CreateOrUpdateBasketAsync(basket, timetolive, ct);
            if (result is null)
                return Result<BasketDto>.Fail(Error.Failure("Basket.CreateOrUpdate.Failed", "Failed to create or update the basket."));

            return Result<BasketDto>.Ok(dto);
        }

        public async Task<Result<bool>> DeleteBasketAsync(string basketId, CancellationToken ct = default)
        {
            var basket = await basketRepository.GetBasketAsync(basketId, ct);
            if (basket is null)
                return Result<bool>.Fail(Error.NotFound("Basket.NotFound", $"Basket with Id {basketId} not found."));

            var result = await basketRepository.DeleteBasketAsync(basketId, ct);
            return result ?
                  Result<bool>.Ok(true)
                : Result<bool>.Fail(Error.Failure("Basket.Delete.Failed", $"Failed to delete the basket with Id {basketId}."));
        }

        public async Task<Result<BasketDto>> GetBasketAsync(string basketId, CancellationToken ct = default)
        {
            var basket = await basketRepository.GetBasketAsync(basketId, ct);
            if (basket is null)
                return Result<BasketDto>.Fail(Error.NotFound("Basket.NotFound", $"Basket with Id {basketId} not found."));
            
            var dto = mapper.Map<BasketDto>(basket);
            return Result<BasketDto>.Ok(dto);
        }
    }
}
