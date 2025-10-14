using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Team
{
    public class PutTeamDto
    {
        public string? Name { get; set; }
        public int? OfficerPlayerId { get; set; }
        
    }
}
