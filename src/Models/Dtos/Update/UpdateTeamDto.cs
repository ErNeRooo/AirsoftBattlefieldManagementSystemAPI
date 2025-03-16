using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update
{
    public class UpdateTeamDto
    {
        [MaxLength(60)]
        public string Name { get; set; }
        public int OfficerPlayerId { get; set; }
    }
}
