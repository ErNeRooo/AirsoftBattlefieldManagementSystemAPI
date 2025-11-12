using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Battle;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Death;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Kill;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Location;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.MapPing;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Order;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace AirsoftBattlefieldManagementSystemAPI.Realtime;

public interface IRoomNotificationClient
{
    Task BattleCreated(BattleDto battleDto);
    Task BattleUpdated(BattleDto battleDto);
    Task BattleDeleted(int battleId);
    
    Task DeathCreated(DeathDto deathDto);
    Task DeathUpdated(DeathDto deathDto);
    Task DeathDeleted(int deathId);
    
    Task KillCreated(KillDto killDto);
    Task KillUpdated(KillDto killDto);
    Task KillDeleted(int killId);
    
    Task LocationCreated(LocationDto locationDto);
    Task LocationUpdated(LocationDto locationDto);
    Task LocationDeleted(int locationId);
    
    Task MapPingCreated(MapPingDto mapPingDto);
    Task MapPingUpdated(MapPingDto mapPingDto);
    Task MapPingDeleted(int mapPingId);
    
    Task OrderCreated(OrderDto orderDto);
    Task OrderUpdated(OrderDto orderDto);
    Task OrderDeleted(int orderId);
    
    Task PlayerUpdated(PlayerDto playerDto);
    Task PlayerLeftTeam(int playerId);
    Task PlayerLeftRoom(int playerId);
    
    Task RoomUpdated(RoomDto roomDto);
    Task RoomJoined(PlayerDto playerDto);
    Task RoomDeleted();
    
    Task TeamCreated(TeamDto teamDto);
    Task TeamUpdated(TeamDto teamDto);
    Task TeamDeleted(int teamId);

    Task ZoneCreated(ZoneDto zoneDto);
    Task ZoneUpdated(ZoneDto zoneDto);
    Task ZoneDeleted(int teamId);
}

[Authorize]
public sealed class RoomNotificationHub : Hub<IRoomNotificationClient>
{
    
}