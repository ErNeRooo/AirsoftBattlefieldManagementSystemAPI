using AirsoftBattlefieldManagementSystemAPI.Models.BattleManagementSystemDbContext;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using Microsoft.AspNetCore.Authorization;

namespace AirsoftBattlefieldManagementSystemAPI.Authorization;

public class PlayerIsInTheSameRoomAsResourceHandler(IBattleManagementSystemDbContext dbContext) : AuthorizationHandler<PlayerIsInTheSameRoomAsResourceRequirement, int?>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PlayerIsInTheSameRoomAsResourceRequirement resourceRequirement,
        int? roomId)
    {
        if (roomId is null) context.Fail();

        string? claimPlayerId = context.User.FindFirst(c => c.Type == "playerId").Value;
        bool isConvertedSuccessfully = int.TryParse(claimPlayerId, out int playerId);
        
        if(!isConvertedSuccessfully) context.Fail();
        
        Player? player = dbContext.Player.FirstOrDefault(player => player.PlayerId == playerId);

        if (player != null && player.RoomId == roomId) context.Succeed(resourceRequirement);
        else context.Fail();

        return Task.CompletedTask;
    }
}