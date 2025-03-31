using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create
{
    public class PostRoomDto
    {
        public int MaxPlayers { get; set; }
        public string? JoinCode { get; set; }
        public string Password { get; set; } = string.Empty;
    }
}
