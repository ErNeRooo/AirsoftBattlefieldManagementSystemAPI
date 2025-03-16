using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update
{
    public class UpdatePlayerDto
    {
        [MaxLength(20)]
        public string? Name { get; set; }
        public bool? IsDead { get; set; }
        public int? TeamId { get; set; }
    }
}
