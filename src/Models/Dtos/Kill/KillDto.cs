using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Kill
{
    public class KillDto
    {
        public int KillId { get; set; }
        public int LocationId { get; set; }
        public int PlayerId { get; set; }
        public int RoomId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public decimal Accuracy { get; set; }
        public short Bearing { get; set; }
        public DateTime Time { get; set; }
    }
}
