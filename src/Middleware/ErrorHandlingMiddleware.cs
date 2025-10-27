using AirsoftBattlefieldManagementSystemAPI.Exceptions;

namespace AirsoftBattlefieldManagementSystemAPI.Middleware
{
    public class ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger) : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }

            catch (WrongPasswordException e)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(e.Message);
            }
            catch (InvalidJoinCodeFormatException e)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(e.Message);
            }
            catch (BadRequestException e)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync(e.Message);
            }
            catch (ForbidException e)
            {
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync(e.Message);
            }
            catch (NotFoundException e)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync(e.Message);
            }
            catch (OnePlayerCannotHaveTwoAccountsException e)
            {
                context.Response.StatusCode = 409;
                await context.Response.WriteAsync(e.Message);
            }
            catch (PlayerAlreadyInsideRoomException e)
            {
                context.Response.StatusCode = 409;
                await context.Response.WriteAsync(e.Message);
            }
            catch (NotASingleAvailableJoinCodeException e)
            {
                logger.LogError(e, e.Message);
                
                context.Response.StatusCode = 503;
                await context.Response.WriteAsync(e.Message);
            }
            
            catch (Exception e)
            {
                logger.LogError(e, e.Message);

                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Something went wrong :/");
            }  
        }
    }
}
