using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update
{
    public class PutRoomDto
    {
        public int? MaxPlayers { get; set; }
        public string? JoinCode { get; set; }
        public string? Password { get; set; } = string.Empty;
        public int? AdminPlayerId { get; set; }
    }
}
