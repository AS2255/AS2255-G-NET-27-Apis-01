using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Application.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

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

        //register
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto, CancellationToken ct = default)
        {
            var result = await authenticationService.RegisterAsync(registerDto, ct);
            return ToActionResult(result);
        }

        // Check Email Exists
        [HttpGet("emailExists/{email}")]
        public async Task<ActionResult<bool>> CheckEmail(string email, CancellationToken ct = default)
        {
            var result = await authenticationService.CheckEmailExistsAsync(email, ct);
            return ToActionResult(result);
        }
        // Get Current User
        [HttpGet("currentUser")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser(CancellationToken ct = default)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException();
            var result = await authenticationService.GetCurrentUserAsync(email, ct);
            return ToActionResult(result);
        }
        // Get Current User Address
        [HttpGet("Address")]
        [Authorize]

        public async Task<ActionResult<AddressDto>> GetCurrentUserAddressAsync(CancellationToken ct = default)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException();
            var result = await authenticationService.GetCurrentUserAddressAsync(email, ct);
            return ToActionResult(result);
        }
        // Update Current User Address
        [HttpPut("Address")]
        [Authorize]

        public async Task<ActionResult<AddressDto>> UpdateCurrentUserAddressAsync(AddressDto addressDto, CancellationToken ct = default)
        {
            var email = User.FindFirstValue(ClaimTypes.Email) ?? throw new UnauthorizedAccessException();
            var result = await authenticationService.UpdateCurrentUserAddressAsync(email, addressDto, ct);
            return ToActionResult(result);
        }
    }
}
