using FirebaseAdmin.Auth;

namespace LOLA_SERVER.API.Middlewares
{
    public class FirebaseAuthenticationMiddleware
    {

        private readonly RequestDelegate _next;

        public FirebaseAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                try
                {
                    var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
                    context.Items["User"] = decodedToken;
                }
                catch
                {
                    context.Response.StatusCode = 401; 
                    await context.Response.WriteAsync("Invalid token");
                    return;
                }
            }

            await _next(context);
        }
    }

    public static class FirebaseAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseFirebaseAuthentication(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FirebaseAuthenticationMiddleware>();
        }

    }
}
