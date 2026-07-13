using E_Commerce.Application.Services.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace E_Commerce.API.Attributes
{
    public class RedisCacheAttribute(int timeInSec) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Get Cache from DI
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<ICacheService>();
            // check if the cached data exists for the current request
            var cacheKey = CreateCacheKey(context.HttpContext.Request);
            var value = await cacheService.GetAsync(cacheKey);

            if (!string.IsNullOrEmpty(value))
            {
                // return response from cache
                context.Result = new ContentResult()
                {
                    Content = value,
                    StatusCode = 200,
                    ContentType = "application/json"
                };
                return;
            }
            else 
            {
                // call Api method and cache the result
                var executed = await next.Invoke();
                if (executed.Result is OkObjectResult okResult)
                { 
                    await cacheService.SetAsync(cacheKey, okResult.Value, TimeSpan.FromSeconds(timeInSec));
                }
            }

            return;
        }

        private static string CreateCacheKey(HttpRequest request)
        {
            var key = new StringBuilder();
            key.Append(request.Path);
            foreach (var item in request.Query)
            {
                key.Append($"{item.Key} | {item.Value}");
            }

            return key.ToString();
        }
    }
}
