using System.ComponentModel.DataAnnotations;
using AirsoftBattlefieldManagementSystemAPI.Models.Entities;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Order;

public class PostOrderDto
{
    public int PlayerId { get; set; }
    public decimal Longitude { get; set; }
    public decimal Latitude { get; set; }
    public decimal Accuracy { get; set; }
    public short Bearing { get; set; }
    public DateTimeOffset Time { get; set; }
    public string? Type { get; set; } = OrderTypes.MOVE;
}

