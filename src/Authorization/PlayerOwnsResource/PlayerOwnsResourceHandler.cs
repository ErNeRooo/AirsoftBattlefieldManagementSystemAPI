using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using Microsoft.AspNetCore.Authorization;

namespace AirsoftBattlefieldManagementSystemAPI.Authorization.PlayerOwnsResource
{
    public class PlayerOwnsResourceHandler(IClaimsHelperService claimsHelper) : AuthorizationHandler<PlayerOwnsResourceRequirement, int>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PlayerOwnsResourceRequirement requirement, 
            int targetPlayerId)
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", context.User);
            
            if (playerId == targetPlayerId) context.Succeed(requirement);
            else context.Fail();

            return Task.CompletedTask;
        }
    }
}
