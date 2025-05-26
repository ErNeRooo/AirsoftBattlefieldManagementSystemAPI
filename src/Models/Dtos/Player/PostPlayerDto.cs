using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Player
{
    public class PostPlayerDto
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
