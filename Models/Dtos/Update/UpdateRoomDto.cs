using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update
{
    public class UpdateRoomDto
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
