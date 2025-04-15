using Bel_Souvenirs.Models;
using Microsoft.AspNetCore.Identity;

namespace Bel_Souvenirs.MiddleWare
{
    public class UserInfoMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(
            HttpContext httpContext,
            UserManager<ApplicationUser> userManager)
        {
            if (httpContext.User?.Identity?.IsAuthenticated == true)
            {
                var user = await userManager.GetUserAsync(httpContext.User);

                if (user != null && !string.IsNullOrEmpty(user.FullName))
                {
                    httpContext.Items["FullName"] = user.FullName;
                }
            }

            await _next(httpContext);
        }
    }
}