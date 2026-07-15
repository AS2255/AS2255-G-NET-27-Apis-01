using E_Commerce.Application.Common;
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
    }
}
