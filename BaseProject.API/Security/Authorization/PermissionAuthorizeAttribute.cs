using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Shared.DTOs.Common;

namespace BaseProject.API.Security.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermissionAuthorizeAttribute(string permission) : Attribute, IAsyncAuthorizationFilter
    {
        public string Permission { get; set; } = permission;

        /// <inheritdoc/>
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (string.IsNullOrEmpty(Permission))
                return;

            IPermissionService permissionService = context.HttpContext.RequestServices.GetRequiredService<IPermissionService>();

            // Allow if authorized
            if (await permissionService.AuthorizeAsync(Permission))
                return;

            // Deny access for Web API
            context.Result = new JsonResult(new ErrorDto
            {
                Message = $"Access denied for permission '{Permission}' to resource '{context.HttpContext.Request.Path}'",
                Code = "401"
            })
            {
                StatusCode = 401
            };
        }
    }
}
