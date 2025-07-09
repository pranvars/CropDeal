using AuthenticationService;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace CustomersAPI
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class CustomAuthenticationAttribute : Attribute, IAuthorizationFilter
    {
        public string Roles { get; set; }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var headers = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            var token = headers?.Split(" ").Last();
            if (token is null)
            {
                context.Result = new JsonResult(
                    new { message = "You are not authorized to use this API" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
            else
            {
                context.HttpContext.Items["Token"] = token;
                var validator = context.HttpContext.RequestServices.GetService<TokenValidator>();
                var user = validator.Validate(token).Result;
                if (user is null)
                {
                    context.Result = new JsonResult(
                        new { message = "You are not authorized to use this API" })
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };
                }
                else
                {
                    var roles = Roles.Split(",").ToList();
                    var roleExists = roles.Exists(c => c == user.Role.RoleName);
                    if (!roleExists)
                    {
                        context.Result = new JsonResult(
                            new { message = "You are not authorized to use this API" })
                        {
                            StatusCode = StatusCodes.Status401Unauthorized
                        };
                    }
                }
            }
        }
    }
}
