namespace AirsoftBattlefieldManagementSystemAPI.Models.Entities;

public interface OrderTypes { 
    public const string MOVE = "MOVE";
    public const string DEFEND = "DEFEND";
}
    
public class Order
{
    public int OrderId { get; set; }
    public string OrderType { get; set; }
    public int PlayerId { get; set; }
    public virtual Player Player { get; set; }
    public int LocationId { get; set; }
    public virtual Location Location { get; set; }
    public int BattleId { get; set; }
    public virtual Battle Battle { get; set; }
}