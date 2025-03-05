using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create
{
    public class CreateDeathDto
    {
        [Required]
        public decimal Longitude { get; set; }
        [Required]
        public decimal Latitude { get; set; }
        [Required]
        public short Accuracy { get; set; }
        [Required]
        public short Bearing { get; set; }
        [Required]
        public DateTime Time { get; set; }
    }
}
