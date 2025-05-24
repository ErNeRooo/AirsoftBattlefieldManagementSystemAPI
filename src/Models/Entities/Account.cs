namespace AirsoftBattlefieldManagementSystemAPI.Models.Entities
{
    public class Account
    {
        public int AccountId { get; set; }
        public int? PlayerId { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        
        public virtual Player Player { get; set; }
    }
}
