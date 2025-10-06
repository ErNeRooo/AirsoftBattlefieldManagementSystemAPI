namespace AirsoftBattlefieldManagementSystemAPI.Models.Entities;

public interface MapPingTypes { 
    public const string ENEMY = "ENEMY";
}
    
public class MapPing
{
    public int MapPingId { get; set; }
    public string Type { get; set; } = MapPingTypes.ENEMY;
    public int PlayerId { get; set; }
    public virtual Player Player { get; set; }
    public int LocationId { get; set; }
    public virtual Location Location { get; set; }
    public int BattleId { get; set; }
    public virtual Battle Battle { get; set; }
}