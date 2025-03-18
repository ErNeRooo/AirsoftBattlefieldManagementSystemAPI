using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update
{
    public class UpdateBattleDto
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
