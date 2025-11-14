using System.ComponentModel.DataAnnotations.Schema;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Entities;

public class ZoneVertex
{
    public int ZoneVertexId { get; set; }
    public int ZoneId { get; set; }
    
    [Column(TypeName = "decimal(8,5)")]
    public decimal Longitude { get; set; }
    
    [Column(TypeName = "decimal(7,5)")]
    public decimal Latitude { get; set; }
    public virtual Zone Zone { get; set; }
}