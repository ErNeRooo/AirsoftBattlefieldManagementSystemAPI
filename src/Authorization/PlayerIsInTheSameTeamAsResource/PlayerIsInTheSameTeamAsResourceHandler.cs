using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using Microsoft.AspNetCore.Authorization;

namespace AirsoftBattlefieldManagementSystemAPI.Authorization.PlayerIsInTheSameTeamAsResource;

public class PlayerIsInTheSameTeamAsResourceHandler(IClaimsHelperService claimsHelper, IDbContextHelperService dbHelper) : AuthorizationHandler<PlayerIsInTheSameTeamAsResourceRequirement, int>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PlayerIsInTheSameTeamAsResourceRequirement resourceRequirement,
        int teamId)
    {
        int playerId = claimsHelper.GetIntegerClaimValue("playerId", context.User);
        
        Player player = dbHelper.Player.FindById(playerId);

        if (player.TeamId == teamId) context.Succeed(resourceRequirement);
        else context.Fail();

        return Task.CompletedTask;
    }
}