using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create
{
    public class CreateBattleDto
    {
        [Required]
        public string Name { get; set; }
        public bool IsActive { get; set; } = false;
        [Required]
        public int RoomId { get; set; }
    }
}
