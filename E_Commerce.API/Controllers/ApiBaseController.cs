using E_Commerce.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        [HttpGet]
        public static ActionResult<T> ToActionResult<T> (Result result)
        {
            if (result.IsSuccess)
            {
                return new OkObjectResult(result);
            }
            else
            {
                return ToProblem(result.Errors);
            }
        }

        public static ActionResult<T> ToActionResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                return new OkObjectResult(result.Data);
            }
            else
            {
                return ToProblem(result.Errors);
            }
        }

        protected static ObjectResult ToProblem(IReadOnlyList<Error> errors)
        {
            var FirstError = errors[0];
            var StatusCode = FirstError.ErrorType switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };

            var Problems = new ProblemDetails
            {
                Title = FirstError.Code,
                Detail = FirstError.Description,
                Status = StatusCode,
                Extensions = { ["Errors"] = errors}
            };

            return new ObjectResult(Problems) { StatusCode = StatusCode };
        }
    }
}
