using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create
{
    public class CreateRoomDto
    {
        [Required]
        public int AdminPlayerId { get; set; }
        [Required]
        public int MaxPlayers { get; set; }
        [Length(6, 6)]
        public string JoinCode { get; set; }
        public string Password { get; set; }
    }
}
