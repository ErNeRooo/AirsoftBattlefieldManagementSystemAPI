
namespace AirsoftBattlefieldManagementSystemAPI.Middleware
{
    public class AuthorizationMiddleware : IMiddleware
    {
        public Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            throw new Exception("AuthorizationMiddleware is not implemented yet. " +
                "Please implement it to handle authorization logic for the application.");
        }
    }
}
