using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using Microsoft.AspNetCore.Authorization;

namespace AirsoftBattlefieldManagementSystemAPI.Authorization.JwtPlayerIdHasExistingPlayerEntity
{
    public class JwtPlayerIdHasExistingPlayerEntityHandler(IBattleManagementSystemDbContext dbContext, IClaimsHelperService claimsHelper, IDbContextHelperService dbHelper) : AuthorizationHandler<JwtPlayerIdHasExistingPlayerEntityRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, JwtPlayerIdHasExistingPlayerEntityRequirement requirement)
        {
            int playerIdFromClaims = claimsHelper.GetIntegerClaimValue("playerId", context.User);
            
            bool isPlayerWithIdFromJwtExisting = dbContext.Player.Any(player => player.PlayerId == playerIdFromClaims);

            if (isPlayerWithIdFromJwtExisting) context.Succeed(requirement);
            else context.Fail();

            return Task.CompletedTask;
        }
    }
}
