using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update
{
    public class PutDeathDto
    {
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public short? Accuracy { get; set; }
        public short? Bearing { get; set; }
        public DateTime? Time { get; set; }
    }
}
