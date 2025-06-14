using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Battle
{
    public class PostBattleDto
    {
        public string Name { get; set; }
        public int RoomId { get; set; }
    }
}
