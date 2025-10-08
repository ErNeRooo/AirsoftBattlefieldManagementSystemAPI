using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using Microsoft.AspNetCore.Authorization;

namespace AirsoftBattlefieldManagementSystemAPI.Authorization.IsAdminOrOfficerOfTargetPlayer
{
    public class IsAdminOrOfficerOfTargetPlayerHandler(IClaimsHelperService claimsHelper) : AuthorizationHandler<IsAdminOrOfficerOfTargetPlayerRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            IsAdminOrOfficerOfTargetPlayerRequirement requirement
            )
        {
            int playerId = claimsHelper.GetIntegerClaimValue("playerId", context.User);
            
            if (playerId == requirement.OfficerPlayerId || playerId == requirement.AdminPlayerId) context.Succeed(requirement);
            else context.Fail();

            return Task.CompletedTask;
        }
    }
}
