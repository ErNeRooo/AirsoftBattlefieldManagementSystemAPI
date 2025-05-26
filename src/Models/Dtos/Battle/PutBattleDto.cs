using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Battle
{
    public class PutBattleDto
    {
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
    }
}
