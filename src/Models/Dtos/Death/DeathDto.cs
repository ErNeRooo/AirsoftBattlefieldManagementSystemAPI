using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Death
{
    public class DeathDto
    {
        public int DeathId { get; set; }
        public int LocationId { get; set; }
        public int PlayerId { get; set; }
        public int BattleId { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public decimal Accuracy { get; set; }
        public short Bearing { get; set; }
        public DateTimeOffset Time { get; set; }
    }
}
