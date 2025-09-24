using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace PARS_Inhouse_Systems_API.Config.AnyPonit
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class AuthorizeRolesAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string[] _roles;

        public AuthorizeRolesAttribute(params string[] roles)
        {
            _roles = roles;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var httpContext = context.HttpContext;
            var config = httpContext.RequestServices.GetService(typeof(IConfiguration)) as IConfiguration;

            if (config == null)
            {
                context.Result = new Microsoft.AspNetCore.Mvc.ForbidResult();
                return;
            }

            if (httpContext.Request.Headers.TryGetValue("Authorization", out var apiKey))
            {
                var webApiKey = config["ApiSettings:WebApiKey"];
                var mobileApiKey = config["ApiSettings:MobileApiKey"];
                var adminApiKey = config["ApiSettings:AdminApiKey"];

                if (apiKey == webApiKey || apiKey == mobileApiKey || apiKey == adminApiKey)
                {
                    return;
                }
            }

            var user = httpContext.User;
            if (user?.Identity == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new Microsoft.AspNetCore.Mvc.ForbidResult();
                return;
            }

            var userRoles = user.Claims
                                .Where(c => c.Type == ClaimTypes.Role)
                                .Select(c => c.Value.ToLower());

            if (!_roles.Any(role => userRoles.Contains(role.ToLower())))
            {
                context.Result = new Microsoft.AspNetCore.Mvc.ForbidResult();
            }

            await Task.CompletedTask;
        }
    }
}
