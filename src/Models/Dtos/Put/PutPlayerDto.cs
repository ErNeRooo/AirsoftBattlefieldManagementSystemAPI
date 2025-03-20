using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update
{
    public class PutPlayerDto
    {
        public string Name { get; set; }
        public bool IsDead { get; set; }
        public int TeamId { get; set; }
        public int RoomId { get; set; }
        public int AccountId { get; set; }
    }
}
