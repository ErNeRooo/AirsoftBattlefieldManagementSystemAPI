using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create
{
    public class CreateBattleDto
    {
        public string Name { get; set; }
        public bool IsActive { get; set; } = false;
        public int RoomId { get; set; }
    }
}
