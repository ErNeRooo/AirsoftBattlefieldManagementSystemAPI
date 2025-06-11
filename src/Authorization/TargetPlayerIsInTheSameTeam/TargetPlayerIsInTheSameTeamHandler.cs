using AirsoftBattlefieldManagementSystemAPI.Authorization.PlayerIsInTheSameRoomAsResource;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using Microsoft.AspNetCore.Authorization;

namespace AirsoftBattlefieldManagementSystemAPI.Authorization.TargetPlayerIsInTheSameTeam;

public class TargetPlayerIsInTheSameTeamHandler(IDbContextHelperService dbHelper) : AuthorizationHandler<TargetPlayerIsInTheSameTeamRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TargetPlayerIsInTheSameTeamRequirement resourceRequirement)
    {
        int playerId = resourceRequirement.PlayerId;
        int teamId = resourceRequirement.TeamId;
        
        Player player = dbHelper.FindPlayerById(playerId);

        if (player.TeamId == teamId) context.Succeed(resourceRequirement);
        else context.Fail();

        return Task.CompletedTask;
    }
}