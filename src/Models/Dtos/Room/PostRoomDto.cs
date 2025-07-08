using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Room
{
    public class PostRoomDto
    {
        public int MaxPlayers { get; set; }
        public string? JoinCode { get; set; }
        public string? Password { get; set; } = "";
    }
}
