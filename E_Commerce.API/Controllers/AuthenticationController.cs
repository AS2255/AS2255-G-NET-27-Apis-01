using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Application.Services.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    public class AuthenticationController(IAuthenticationService authenticationService) : ApiBaseController
    {

        //login
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto, CancellationToken ct = default)
        {
            var result = await authenticationService.LoginAsync(loginDto, ct);
            return ToActionResult(result);
        }


    }
}
