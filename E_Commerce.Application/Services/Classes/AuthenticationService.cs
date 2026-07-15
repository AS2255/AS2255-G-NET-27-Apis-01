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
    public class AuthenticationService(IIdentityService identityService) : IAuthenticationService
    {
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

            return Result<UserDto>.Ok(new UserDto()
            {
                Email = user.Email,
                Token = "TODO",
                DisplayName = user.DisplayName
            });
        }
    }
}
