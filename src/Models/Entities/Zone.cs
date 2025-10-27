namespace AirsoftBattlefieldManagementSystemAPI.Models.Entities;

public interface ZoneTypes { 
    public const string SPAWN = "SPAWN";
    public const string NO_FIRE_ZONE = "NO_FIRE_ZONE";
}

public class Zone
{
    public int ZoneId { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public int? BattleId { get; set; }
    public virtual Battle? Battle { get; set; }
    public virtual Team? Team { get; set; }
    public virtual List<ZoneVertex> Vertices { get; set; }
}