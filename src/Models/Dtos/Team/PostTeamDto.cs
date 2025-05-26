using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team
{
    public class PostTeamDto
    {
        public string Name { get; set; }
        public int RoomId { get; set; }
    }
}
