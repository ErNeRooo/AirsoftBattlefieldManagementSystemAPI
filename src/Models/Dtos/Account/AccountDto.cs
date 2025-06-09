using System.ComponentModel.DataAnnotations;

namespace AirsoftBattlefieldManagementSystemAPI.Models.Dtos.Account
{
    public class AccountDto
    {
        public int AccountId { get; set; }
        public int PlayerId { get; set; }
        public string? Email { get; set; }
    }
}
