using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos
{
    public class CreateTeamDto
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        [Required]
        public int RoomId { get; set; }
    }
}
