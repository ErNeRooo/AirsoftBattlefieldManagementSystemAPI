using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Location
{
    public class PostLocationDto
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public decimal Accuracy { get; set; }
        public short Bearing { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
