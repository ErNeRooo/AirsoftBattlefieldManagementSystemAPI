using AirsoftBattlefieldManagementSystemAPI.Authorization.PlayerIsInTheSameRoomAsResource;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using Microsoft.AspNetCore.Authorization;

namespace AirsoftBattlefieldManagementSystemAPI.Authorization.PlayerIsRoomAdminOrTargetTeamOfficer;

public class PlayerIsRoomAdminOrTargetTeamOfficerHandler(IClaimsHelperService claimsHelper, IDbContextHelperService dbHelper) : AuthorizationHandler<PlayerIsRoomAdminOrTargetTeamOfficerRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PlayerIsRoomAdminOrTargetTeamOfficerRequirement resourceRequirement)
    {
        int playerId = claimsHelper.GetIntegerClaimValue("playerId", context.User);
        int teamId = resourceRequirement.TeamId;
        
        Team team = dbHelper.Team.FindById(teamId);
        Room room = dbHelper.Room.FindById(team.RoomId);
        
        if (playerId == team.OfficerPlayerId || playerId == room.AdminPlayerId) context.Succeed(resourceRequirement);
        else context.Fail();

        return Task.CompletedTask;
    }
}