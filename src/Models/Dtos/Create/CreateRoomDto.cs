using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create
{
    public class CreateRoomDto
    {
        [Required]
        public int MaxPlayers { get; set; }
        public int JoinRoomNumber { get; set; }
    }
}
