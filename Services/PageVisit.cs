using System.Threading.Tasks;
using samadApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace samadApp.PageVisit{
    public class CheckUserMiddleware
    {
        private readonly ILogger<CheckUserMiddleware> _logger;
        private readonly RequestDelegate _next;

        public CheckUserMiddleware(RequestDelegate next,ILogger<CheckUserMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<MvcFprojectContext> userManager)
        {
            using (var serviceScope = context.RequestServices.CreateScope())
            {
                userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<MvcFprojectContext>>();
                if (context.User.Identity.IsAuthenticated)
                {
                    
                    var user = await userManager.GetUserAsync(context.User);
                    if (user != null)
                    {
                        string currentPath = context.Request.Path;
                        _logger.LogInformation(currentPath);
                    }
                }
            }

            await _next(context);
        }
    }

}
