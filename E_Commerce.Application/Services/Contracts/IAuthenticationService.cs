using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services.Contracts
{
    public interface IAuthenticationService
    {
        // Login
        // Email + Password ---> (Token - Email - DisplayName)
        Task<Result<UserDto>> LoginAsync(LoginDto loginDto, CancellationToken ct = default);
        //Register
        Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default);
        Task<Result<bool>> CheckEmailExistsAsync(string email, CancellationToken ct = default);
        Task<Result<UserDto>> GetCurrentUserAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDto>> GetCurrentUserAddressAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDto>> UpdateCurrentUserAddressAsync(string email, AddressDto addressDto, CancellationToken ct = default);
    }
}
