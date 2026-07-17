using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Application.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services.Classes
{
    public class AuthenticationService(IIdentityService identityService, ITokenService tokenService) : IAuthenticationService
    {
        public async Task<Result<bool>> CheckEmailExistsAsync(string email, CancellationToken ct = default)
        {
            var result = await identityService.FindUserByEmailAsync(email, ct);
            if (result.IsSuccess)
                return Result<bool>.Ok(true);

            return Result<bool>.Fail(result.Errors);
        }

        public async Task<Result<AddressDto>> GetCurrentUserAddressAsync(string email, CancellationToken ct = default)
        {
            return await identityService.GetCurrentUserAddressAsync(email, ct);
        }

        public async Task<Result<UserDto>> GetCurrentUserAsync(string email, CancellationToken ct = default)
        {
            var userResult = await identityService.FindUserByEmailAsync(email, ct);
            if (userResult.IsSuccess)
            {
                var user = userResult.Data;
                var rolesResult = await identityService.GetUserRoleAsync(user.Email, ct);
                var token = await tokenService.CreateTokenAsync(user.Id, user.Email, user.UserName, rolesResult.Data);
                return Result<UserDto>.Ok(new UserDto()
                {
                    DisplayName = user.DisplayName,
                    Email = user.Email,
                    Token = token
                });
            }
            else
            {
                return Result<UserDto>.Fail(userResult.Errors);
            }
        }

        public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto, CancellationToken ct = default)
        {
            // Check Email
            var userResult = await identityService.FindUserByEmailAsync(loginDto.Email, ct);

            if (!userResult.IsSuccess)
                return Result<UserDto>.Fail(userResult.Errors);

            // Check Passward
            var passwardResult = await identityService.CheckPasswordAsync(loginDto.Email, loginDto.Password, ct);

            if (!passwardResult.IsSuccess)
                return Result<UserDto>.Fail(passwardResult.Errors);

            //Rturn UserDto

            var user = userResult.Data;

            var rolesResult = await identityService.GetUserRoleAsync(user.Email, ct);

            var token = await tokenService.CreateTokenAsync(user.Id, user.Email, user.UserName, rolesResult.Data);

            return Result<UserDto>.Ok(new UserDto()
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = token
            });
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto, CancellationToken ct = default)
        {
            // Create User
            var createUserResult = await identityService.CreateUserAsync(registerDto, ct);
            if (!createUserResult.IsSuccess)
                return Result<UserDto>.Fail(createUserResult.Errors);

            var user = createUserResult.Data;

            var rolesResult = await identityService.GetUserRoleAsync(user.Email, ct);

            var token = await tokenService.CreateTokenAsync(user.Id, user.Email, user.UserName, rolesResult.Data);

            return Result<UserDto>.Ok(new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                Token = token
            });
        }

        public async Task<Result<AddressDto>> UpdateCurrentUserAddressAsync(string email, AddressDto addressDto, CancellationToken ct = default)
        {
            return await identityService.UpdateCurrentUserAddressAsync(email, addressDto, ct);
        }
    }
}
