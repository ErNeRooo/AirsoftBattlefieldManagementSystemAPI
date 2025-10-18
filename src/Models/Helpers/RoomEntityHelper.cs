using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Helpers;

public static class RoomEntityHelper
{
    public static IEnumerable<string> GetTeamPlayerIdsWithoutSelf(this Room room, int? teamId, int selfPlayerId)
    {
        return room.Players
            .Where(p => p.TeamId == teamId && p.PlayerId != selfPlayerId)
            .Select(p => p.PlayerId.ToString());
    }
    
    public static IEnumerable<string> GetAllPlayerIdsWithoutSelf(this Room room, int selfPlayerId)
    {
        return room.Players
            .Where(p => p.PlayerId != selfPlayerId)
            .Select(p => p.PlayerId.ToString());
    }
}