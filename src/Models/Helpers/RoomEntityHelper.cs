using AirsoftBattlefieldManagementSystemAPI.Models.Entities;
using AirsoftBattlefieldManagementSystemAPI.Services.ClaimsHelperService;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Helpers;

public static class RoomEntityHelper
{
    public static List<string> GetTeamPlayerIdsWithoutSelf(this Room room, int? teamId, int selfPlayerId)
    {
        return room.Players
            .Where(p => p.TeamId == teamId && p.PlayerId != selfPlayerId)
            .Select(p => p.PlayerId.ToString())
            .ToList();
    }
    
    public static List<string> GetAllPlayerIdsWithoutSelf(this Room room, int selfPlayerId)
    {
        return room.Players
            .Where(p => p.PlayerId != selfPlayerId)
            .Select(p => p.PlayerId.ToString())
            .ToList();
    }
}