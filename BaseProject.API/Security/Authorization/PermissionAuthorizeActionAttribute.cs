using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using BaseProject.Application.Common.Interfaces;
using BaseProject.Shared.DTOs.Common;

namespace BaseProject.API.Security.Authorization
{
    [AttributeUsage(AttributeTargets.Method)]
    public class PermissionAuthorizeActionAttribute(string actionName) : Attribute, IAsyncAuthorizationFilter
    {
        public string PermissionAction { get; set; } = actionName;

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (string.IsNullOrEmpty(PermissionAction))
                return;

            PermissionAuthorizeAttribute? permissionAuthorize = context.Filters
                .OfType<PermissionAuthorizeAttribute>()
                .FirstOrDefault();

            if (permissionAuthorize == null || string.IsNullOrEmpty(permissionAuthorize.Permission))
                return;

            IPermissionService permissionService = context.HttpContext.RequestServices.GetRequiredService<IPermissionService>();

            if (await permissionService.AuthorizeActionAsync(permissionAuthorize.Permission, PermissionAction))
                return;

            // For API calls, return JSON with proper HTTP status code
            context.Result = new JsonResult(new ErrorDto
            {
                Message = $"Access denied to resource '{context.HttpContext.Request.Path}'",
                Code = "401"
            })
            {
                StatusCode = 401
            };
        }
    }
}
