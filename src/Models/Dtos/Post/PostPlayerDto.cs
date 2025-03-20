using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create
{
    public class PostPlayerDto
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
    }
}
