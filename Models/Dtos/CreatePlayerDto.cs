using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos
{
    public class CreatePlayerDto
    {
        [Required]
        [MaxLength(20)]
        public string? Name { get; set; }

        public bool IsDead { get; set; } = false;
        [Required]
        public int? TeamId { get; set; }
    }
}
