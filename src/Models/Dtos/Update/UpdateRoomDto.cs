using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update
{
    public class UpdateRoomDto
    {
        public int? MaxPlayers { get; set; }
        [Length(6,6)]
        public string? JoinCode { get; set; }
        public string? Password { get; set; }
        public int? AdminPlayerId { get; set; }
    }
}
