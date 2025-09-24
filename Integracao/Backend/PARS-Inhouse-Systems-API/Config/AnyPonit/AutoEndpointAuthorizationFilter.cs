using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PARS.Inhouse.Systems.Application.Interfaces.AnyPoint;
using PARS.Inhouse.Systems.Infrastructure.Data.DbContext.SQLSERVE;
using System.Security.Claims;

namespace PARS_Inhouse_Systems_API.Config.AnyPonit
{
    public class AutoEndpointAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly IEndpointPermissionService _permissionService;
        private readonly Context _context;

        public AutoEndpointAuthorizationFilter(
            IEndpointPermissionService permissionService,
            Context context)
        {
            _permissionService = permissionService;
            _context = context;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var requestedUrl = context.HttpContext.Request.Path.ToString().ToUpper();
            if (requestedUrl.StartsWith("/API"))
            {
                requestedUrl = requestedUrl[4..];
            }
            if (string.IsNullOrEmpty(requestedUrl))
            {
                context.Result = new ForbidResult();
                return;
            }

            string acao = context.HttpContext.Request.Method.ToLower() switch
            {
                "get" => "read",
                "post" or "put" => "write",
                "delete" => "delete",
                _ => string.Empty
            };

            if (string.IsNullOrEmpty(acao))
            {
                context.Result = new ForbidResult();
                return;
            }

            var userId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new ForbidResult();
                return;
            }

            //var endpoint = await _context.AnyPointUserEndpointPermission.Include(e => e.Menu).FirstOrDefaultAsync(e => e.Menu != null && e.Menu.Rota != null && e.Menu.Rota
            //                                                                                 .ToUpper() == requestedUrl);

            //if (endpoint == null)
            //{
            //    var temPermissao = await _permissionService.HasPermissionAsync(userId, acao);
            //    if (!temPermissao)
            //    {
            //        context.Result = new ForbidResult();
            //        return;
            //    }
            //}
        }
    }

}
