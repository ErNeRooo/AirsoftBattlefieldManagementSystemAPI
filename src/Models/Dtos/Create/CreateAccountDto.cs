using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Create
{
    public class CreateAccountDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
