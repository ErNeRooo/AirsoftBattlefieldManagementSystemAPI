using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Update
{
    public class UpdateAccountDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
