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
            string? claimPlayerId = context.User.FindFirst(c => c.Type == "playerId").Value;
            bool isCovertedSuccessfully = int.TryParse(claimPlayerId, out int playerId);
            
            if (isCovertedSuccessfully && playerId == targetPlayerId) context.Succeed(requirement);
            else context.Fail();

            return Task.CompletedTask;
        }
    }
}
