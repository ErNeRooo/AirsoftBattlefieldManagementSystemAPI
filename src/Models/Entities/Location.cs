using System.ComponentModel.DataAnnotations.Schema;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Entities
{
    public class Location
    {
        public int LocationId { get; set; }
        
        [Column(TypeName = "decimal(8,5)")]
        public decimal Longitude { get; set; }
        
        [Column(TypeName = "decimal(7,5)")]
        public decimal Latitude { get; set; }
        public short Accuracy { get; set; }
        public short Bearing { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
