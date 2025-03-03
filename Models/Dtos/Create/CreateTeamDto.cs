using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create
{
    public class CreateTeamDto
    {
        [Required]
        [MaxLength(60)]
        public string Name { get; set; }
        [Required]
        public int RoomId { get; set; }
    }
}
