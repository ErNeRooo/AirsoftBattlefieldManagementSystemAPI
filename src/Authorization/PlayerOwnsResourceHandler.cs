using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace AirsoftBattlefieldManagementSystemAPI.Authorization
{
    public class PlayerOwnsResourceHandler() : AuthorizationHandler<PlayerOwnsResourceRequirement, int>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PlayerOwnsResourceRequirement requirement, 
            int targetPlayerId)
        {
            string? claimValue = context.User.FindFirst(c => c.Type == "playerId").Value;

            if (claimValue is null) context.Fail();

            int claimPlayerId = int.Parse(claimValue);

            if (targetPlayerId == claimPlayerId)
            {
                context.Succeed(requirement);
            };

            return Task.CompletedTask;
        }
    }
}
