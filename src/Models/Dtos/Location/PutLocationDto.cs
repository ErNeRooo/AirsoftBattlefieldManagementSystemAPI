using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Location
{
    public class PutLocationDto
    {
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public short? Accuracy { get; set; }
        public short? Bearing { get; set; }
        public DateTime? Time { get; set; }
    }
}
