using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace AirsoftBattlefieldManagementSystemAPI.Realtime;

public class PlayerIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        return connection.User?.Claims.FirstOrDefault(c => c.Type == "playerId")?.Value;
    }
}