using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update
{
    public class PutBattleDto
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
