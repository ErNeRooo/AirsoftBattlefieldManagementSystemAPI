using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;
using AirsoftBattlefieldManagementSystemAPI.Services.DbContextHelperService;
using Microsoft.AspNetCore.Authorization;

namespace AirsoftBattlefieldManagementSystemAPI.Authorization.PlayerIsInTheSameRoomAsResource;

public class PlayerIsInTheSameRoomAsResourceHandler(IClaimsHelperService claimsHelper, IDbContextHelperService dbHelper) : AuthorizationHandler<PlayerIsInTheSameRoomAsResourceRequirement, int>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PlayerIsInTheSameRoomAsResourceRequirement resourceRequirement,
        int roomId)
    {
        int playerId = claimsHelper.GetIntegerClaimValue("playerId", context.User);
        
        Player player = dbHelper.FindPlayerById(playerId);

        if (player.RoomId == roomId || player.PlayerId == playerId) context.Succeed(resourceRequirement);
        else context.Fail();

        return Task.CompletedTask;
    }
}