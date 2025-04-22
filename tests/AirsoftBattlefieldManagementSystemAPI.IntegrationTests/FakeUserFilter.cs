using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AirsoftBattlefieldManagementSystemAPI.IntegrationTests;

public class FakeUserFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var claimsPrincipal = new ClaimsPrincipal();
        
        claimsPrincipal.AddIdentity(new ClaimsIdentity(new[]
        {
            new Claim("playerId", 2137.ToString()),
        }));
        
        await next();
    }
}