using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AirsoftBattlefieldManagementSystemAPI.Tests;

public class FakeUserFilter(int playerId) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var claimsPrincipal = new ClaimsPrincipal();
        
        claimsPrincipal.AddIdentity(new ClaimsIdentity([
            new Claim("playerId", playerId.ToString())
        ]));
        
        context.HttpContext.User = claimsPrincipal;
        
        await next();
    }
}