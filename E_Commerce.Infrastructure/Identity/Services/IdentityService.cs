using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Application.Services.Contracts;
using E_Commerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Identity.Services
{
    public class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
    {
        public async Task<Result<bool>> CheckPasswordAsync(string email, string password, CancellationToken ct = default)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
                return Result<bool>.Fail(Error.InvalidCredentials("User.InvalidCredentials", $"Email Or Password Invalid"));

            var result = await userManager.CheckPasswordAsync(user, password);
            return result ?
                Result<bool>.Ok(result)
                :
                Result<bool>.Fail(Error.InvalidCredentials("User.InvalidCredentials", "Email Or Password Invalid"));
        }

        public async Task<Result<IdentityUserResult>> FindUserByEmailAsync(string email, CancellationToken ct = default)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
                return Result<IdentityUserResult>.Fail(Error.NotFound("User.NotFound", $"User With Email {email} Not Found"));

            return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.DisplayName, user.Email, user.UserName));
        }

        public async Task<Result<IdentityUserResult>> CreateUserAsync(RegisterDto registerDto, CancellationToken ct = default)
        {
            var user = new ApplicationUser()
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                DisplayName = registerDto.DisplayName,
                PhoneNumber = registerDto.PhoneNumber
            };

            var createResult = await userManager.CreateAsync(user, registerDto.Password);
            if (!createResult.Succeeded)
            {
                var errors = createResult.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
                return Result<IdentityUserResult>.Fail(errors);
            }

            return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.DisplayName, user.Email, user.UserName));
        }

        public async Task<Result<IReadOnlyList<string>>> GetUserRoleAsync(string email, CancellationToken ct = default)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
                return Result<IReadOnlyList<string>>.Fail(Error.NotFound("User.NotFound", $"User With Email {email} Not Found"));

            var roles = await userManager.GetRolesAsync(user);
            return Result<IReadOnlyList<string>>.Ok(roles.ToList());
        }
    }
}
