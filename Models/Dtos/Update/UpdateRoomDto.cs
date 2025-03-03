using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update
{
    public class UpdateRoomDto
    {
        [Required]
        public int MaxPlayers { get; set; }
    }
}
