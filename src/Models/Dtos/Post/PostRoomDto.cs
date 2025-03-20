using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create
{
    public class PostRoomDto
    {
        public int AdminPlayerId { get; set; }
        public int MaxPlayers { get; set; }
        public string? JoinCode { get; set; }
        public string? Password { get; set; }
    }
}
