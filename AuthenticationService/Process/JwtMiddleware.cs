using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.Process
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next) { _next = next; }

        public async Task Invoke(HttpContext context, [FromServices] TokenManager _tokenManager)
        {

            var headers = context.Request.Headers["Authorization"].FirstOrDefault();
            var token = headers?.Split(" ").Last();
            if (token is not null)
            {
                var email = _tokenManager.ValidateToken(token);
                if (email is not null)
                {
                    context.Items["Email"] = email;
                }
            }

            await _next(context);
        }
    }
}
