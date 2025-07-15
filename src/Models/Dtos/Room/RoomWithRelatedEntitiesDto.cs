using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Battle;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player;
using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room;

public class RoomWithRelatedEntitiesDto
{
    public int RoomId { get; set; }
    public int MaxPlayers { get; set; }
    public string JoinCode { get; set; }
    public PlayerDto? AdminPlayer { get; set; }
    public BattleDto Battle { get; set; }
    public List<PlayerDto> Players { get; set; }
    public List<TeamDto> Teams { get; set; }
}