using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos
{
    public class UpdateTeamDto
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
