using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Death
{
    public class PostDeathDto
    {
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public decimal Accuracy { get; set; }
        public short Bearing { get; set; }
        public DateTime Time { get; set; }
    }
}
