using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using AwesomePizzaDAL.Entities;

namespace AwesomePizzaAPI.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IList<string> _roles;

        public AuthorizeAttribute(params string[] roles)
        {
            _roles = roles ?? new string[] { };
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // skip authorization if action is decorated with [AllowAnonymous] attribute
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            // authorization
            var isAuth = context.HttpContext.User?.Identity?.IsAuthenticated ?? false;

            if (!isAuth)
            {
                // not logged in or role not authorized
                context.Result = new JsonResult(new HttpMessage() { success = false, message = "Accesso negato" }) { StatusCode = StatusCodes.Status401Unauthorized };
            }
        }
    }
}