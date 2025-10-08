using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using Microsoft.AspNetCore.Authorization;

namespace AirsoftBattlefieldManagementSystemAPI.Authorization.IsNotSelf
{
    public class IsNotSelfHandler(IClaimsHelperService claimsHelper) : AuthorizationHandler<IsNotSelfRequirement, int>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsNotSelfRequirement requirement, 
            int targetPlayerId)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", context.User);
            
            if (playerId != targetPlayerId) context.Succeed(requirement);
            else context.Fail();

            return Task.CompletedTask;
        }
    }
}
