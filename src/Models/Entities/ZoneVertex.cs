namespace AirsoftBattlefieldManagementSystemAPI.Models.Entities;

public class ZoneVertex
{
    public int ZoneVertexId { get; set; }
    public int ZoneId { get; set; }
    public decimal Longitude { get; set; }
    public decimal Latitude { get; set; }
    public virtual Zone Zone { get; set; }
}