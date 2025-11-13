using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Battle;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Death;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Kill;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Location;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.MapPing;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Order;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;

public class RoomWithRelatedEntitiesDto
{
    public int RoomId { get; set; }
    public int MaxPlayers { get; set; }
    public string JoinCode { get; set; }
    public PlayerDto? AdminPlayer { get; set; }
    public BattleDto? Battle { get; set; }
    public List<PlayerDto> Players { get; set; }
    public List<TeamDto> Teams { get; set; }
    public List<LocationDto> Locations { get; set; }
    public List<KillDto> Kills { get; set; }
    public List<DeathDto> Deaths { get; set; }
    public List<ZoneDto> Zones { get; set; }
    public List<OrderDto> Orders { get; set; }
    public List<MapPingDto> MapPings { get; set; }
}