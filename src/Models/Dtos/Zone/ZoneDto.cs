using AirsoftBattlefieldManagementSystemAPI.Models.Dtos.ZoneVertex;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Zone;

public class ZoneDto
{
    public int ZoneId { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public int BattleId { get; set; }
    public List<ZoneVertexDto> Vertices { get; set; }
}