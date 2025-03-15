using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update
{
    public class UpdateRoomDto
    {
        public int MaxPlayers { get; set; }
        public int JoinRoomNumber { get; set; }
    }
}
