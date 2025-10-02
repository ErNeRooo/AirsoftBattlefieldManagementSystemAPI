using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Order;

public class OrderDto
{
    public int OrderId { get; set; }
    public int LocationId { get; set; }
    public int PlayerId { get; set; }
    public int BattleId { get; set; }
    public decimal Longitude { get; set; }
    public decimal Latitude { get; set; }
    public decimal Accuracy { get; set; }
    public short Bearing { get; set; }
    public DateTimeOffset Time { get; set; }
    public string Type { get; set; }
}

