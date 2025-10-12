using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.ZoneVertex;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;

public class PostZoneDto
{
    public string Type { get; set; }
    public string Name { get; set; }
    public int BattleId { get; set; }
    public List<PostZoneVertexDto> Vertices { get; set; }
}